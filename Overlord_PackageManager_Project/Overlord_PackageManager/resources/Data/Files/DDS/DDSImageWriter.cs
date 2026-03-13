using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.Data.Files.DDS
{
    public static class DDSImageWriter
    {
        private const uint DDS_MAGIC = 0x20534444;

        public static void Write(Stream output, DDSFile file)
        {
            using BinaryWriter bw = new(output, Encoding.ASCII, true);
            {
                WriteHeader(bw, file);

                foreach (DDSMipFace? face in file.Faces.OrderBy(f => f.FaceIndex).ThenBy(f => f.MipIndex))
                {
                    bw.Write(face.Data);
                }
            }
        }

        private static void WriteHeader(BinaryWriter bw, DDSFile file)
        {
            bw.Write(DDS_MAGIC);
            bw.Write(124u);

            uint flags = 0x1 | 0x2 | 0x4 | 0x1000 | 0x20000;
            flags |= file.Format == DDSFormat.DXT1 ||
                     file.Format == DDSFormat.DXT3 ||
                     file.Format == DDSFormat.DXT5
                     ? 0x80000u
                     : 0x8u;

            bw.Write(flags);
            bw.Write(file.Height);
            bw.Write(file.Width);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(file.MipCount);

            for (int i = 0; i < 11; i++)
                bw.Write(0u);

            WritePixelFormat(bw, file.Format);

            uint caps = 0x1000;
            if (file.MipCount > 1)
                caps |= 0x8 | 0x400000;

            if (file.IsCubemap)
                caps |= 0x8;

            bw.Write(caps);

            if (file.IsCubemap)
                bw.Write(0x200 | 0x400 | 0x800 | 0x1000 | 0x2000 | 0x4000 | 0x8000);
            else
                bw.Write(0u);

            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
        }

        private static void WritePixelFormat(BinaryWriter bw, DDSFormat format)
        {
            bw.Write(32u);

            if (format == DDSFormat.DXT1 ||
                format == DDSFormat.DXT3 ||
                format == DDSFormat.DXT5)
            {
                bw.Write(0x4u);
                bw.Write(Encoding.ASCII.GetBytes(format.ToString()));
                bw.Write(0u);
                bw.Write(0u);
                bw.Write(0u);
                bw.Write(0u);
                bw.Write(0u);
            }
            else if (format == DDSFormat.UncompressedRGB)
            {
                bw.Write(0x40u);
                bw.Write(0u);
                bw.Write(24u);
                bw.Write(0x00FF0000u);
                bw.Write(0x0000FF00u);
                bw.Write(0x000000FFu);
                bw.Write(0u);
            }
            else
            {
                bw.Write(0x41u);
                bw.Write(0u);
                bw.Write(32u);
                bw.Write(0x00FF0000u);
                bw.Write(0x0000FF00u);
                bw.Write(0x000000FFu);
                bw.Write(0xFF000000u);
            }
        }
    }
}