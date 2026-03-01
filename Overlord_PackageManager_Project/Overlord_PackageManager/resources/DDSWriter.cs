using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.IO;
using System.Text;

public static class DDSWriter
{
    private const uint DDS_MAGIC = 0x20534444; // "DDS "

    // DDSD flags
    private const uint DDSD_CAPS = 0x1;
    private const uint DDSD_HEIGHT = 0x2;
    private const uint DDSD_WIDTH = 0x4;
    private const uint DDSD_PITCH = 0x8;
    private const uint DDSD_PIXELFORMAT = 0x1000;
    private const uint DDSD_MIPMAPCOUNT = 0x20000;
    private const uint DDSD_LINEARSIZE = 0x80000;

    // CAPS
    private const uint DDSCAPS_COMPLEX = 0x8;
    private const uint DDSCAPS_TEXTURE = 0x1000;
    private const uint DDSCAPS_MIPMAP = 0x400000;

    // CAPS2
    private const uint DDSCAPS2_CUBEMAP = 0x200;
    private const uint DDSCAPS2_POSX = 0x400;
    private const uint DDSCAPS2_NEGX = 0x800;
    private const uint DDSCAPS2_POSY = 0x1000;
    private const uint DDSCAPS2_NEGY = 0x2000;
    private const uint DDSCAPS2_POSZ = 0x4000;
    private const uint DDSCAPS2_NEGZ = 0x8000;

    public static uint CalculateMipMapCount(uint width, uint height)
    {
        uint maxDim = Math.Max(width, height);
        return (uint)Math.Floor(Math.Log2(maxDim)) + 1;
    }

    public static uint CalculatePitchOrLinearSize(uint width, uint height, DDSFormat format)
    {
        switch (format)
        {
            case DDSFormat.UncompressedRGB:
                return width * 3;

            case DDSFormat.UncompressedRGBA:
                return width * 4;

            case DDSFormat.DXT1:
                return Math.Max(1, width / 4) *
                       Math.Max(1, height / 4) * 8;

            case DDSFormat.DXT3:
            case DDSFormat.DXT5:
                return Math.Max(1, width / 4) *
                       Math.Max(1, height / 4) * 16;

            default:
                throw new NotSupportedException("Unsupported DDS format");
        }
    }

    public static byte[] CreateDDSHeader(uint width, uint height, uint mipMapCount, DDSFormat format, bool isCubemap)
    {
        byte[] header = new byte[128];

        using var ms = new MemoryStream(header);
        using var bw = new BinaryWriter(ms);

        bw.Write(DDS_MAGIC);
        bw.Write(124);

        bool compressed =
            format == DDSFormat.DXT1 ||
            format == DDSFormat.DXT3 ||
            format == DDSFormat.DXT5;

        uint pitchOrLinear = CalculatePitchOrLinearSize(width, height, format);

        uint flags = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT | DDSD_MIPMAPCOUNT;

        flags |= compressed ? DDSD_LINEARSIZE : DDSD_PITCH;

        bw.Write(flags);
        bw.Write(height);
        bw.Write(width);
        bw.Write(pitchOrLinear);
        bw.Write(0);
        bw.Write(mipMapCount);

        for (int i = 0; i < 11; i++)
            bw.Write(0);

        WritePixelFormat(bw, format);

        uint caps = DDSCAPS_TEXTURE;

        if (mipMapCount > 1)
            caps |= DDSCAPS_COMPLEX | DDSCAPS_MIPMAP;

        if (isCubemap)
            caps |= DDSCAPS_COMPLEX;

        bw.Write(caps);

        if (isCubemap)
        {
            bw.Write(
                DDSCAPS2_CUBEMAP |
                DDSCAPS2_POSX | DDSCAPS2_NEGX |
                DDSCAPS2_POSY | DDSCAPS2_NEGY |
                DDSCAPS2_POSZ | DDSCAPS2_NEGZ);
        }
        else
        {
            bw.Write(0);
        }

        bw.Write(0);
        bw.Write(0);
        bw.Write(0);

        return header;
    }

    private static void WritePixelFormat(BinaryWriter bw, DDSFormat format)
    {
        bw.Write(32u);

        switch (format)
        {
            case DDSFormat.DXT1:
            case DDSFormat.DXT3:
            case DDSFormat.DXT5:
                bw.Write(0x4u);
                bw.Write(Encoding.ASCII.GetBytes(format.ToString()));
                bw.Write(0u);
                bw.Write(0u);
                bw.Write(0u);
                bw.Write(0u);
                bw.Write(0u);
                break;

            case DDSFormat.UncompressedRGB:
                bw.Write(0x40u);
                bw.Write(0u);
                bw.Write(24u);
                bw.Write(0x00FF0000u);
                bw.Write(0x0000FF00u);
                bw.Write(0x000000FFu);
                bw.Write(0u);
                break;

            case DDSFormat.UncompressedRGBA:
                bw.Write(0x41u);
                bw.Write(0u);
                bw.Write(32u);
                bw.Write(0x00FF0000u);
                bw.Write(0x0000FF00u);
                bw.Write(0x000000FFu);
                bw.Write(0xFF000000u);
                break;

            default:
                throw new NotSupportedException();
        }
    }
}