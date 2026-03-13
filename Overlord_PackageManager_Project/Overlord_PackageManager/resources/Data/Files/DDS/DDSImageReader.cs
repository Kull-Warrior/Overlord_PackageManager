using System.IO;

namespace Overlord_PackageManager.resources.Data.Files.DDS
{
    public static class DDSImageReader
    {
        public static DDSFile Read(byte[] fileBytes)
        {
            using MemoryStream ms = new(fileBytes);
            using BinaryReader br = new(ms);
            {
                string magic = new string(br.ReadChars(4));
                if (magic != "DDS ")
                    throw new InvalidDataException("Invalid DDS file.");

                br.BaseStream.Position = 12;
                uint height = br.ReadUInt32();
                uint width = br.ReadUInt32();

                br.BaseStream.Position = 28;
                uint mipCount = br.ReadUInt32();
                if (mipCount == 0)
                    mipCount = DDSMath.CalculateMipCount(width, height);

                br.BaseStream.Position = 80;
                uint pfFlags = br.ReadUInt32();
                string fourCC = new string(br.ReadChars(4));

                DDSFormat format;

                if ((pfFlags & 0x4) != 0)
                {
                    format = fourCC switch
                    {
                        "DXT1" => DDSFormat.DXT1,
                        "DXT3" => DDSFormat.DXT3,
                        "DXT5" => DDSFormat.DXT5,
                        _ => throw new NotSupportedException($"Unsupported format {fourCC}")
                    };
                }
                else
                {
                    br.BaseStream.Position = 88;
                    uint bitCount = br.ReadUInt32();
                    format = bitCount == 32
                        ? DDSFormat.UncompressedRGBA
                        : DDSFormat.UncompressedRGB;
                }

                br.BaseStream.Position = 112;
                uint caps2 = br.ReadUInt32();
                bool isCubemap = (caps2 & 0x200) != 0;

                br.BaseStream.Position = 128;

                int faces = isCubemap ? 6 : 1;
                List<DDSMipFace> mipFaces = new();

                for (int face = 0; face < faces; face++)
                {
                    uint w = width;
                    uint h = height;

                    for (int mip = 0; mip < mipCount; mip++)
                    {
                        uint size = DDSMath.CalculateMipByteSize(w, h, format);
                        byte[] data = br.ReadBytes((int)size);

                        mipFaces.Add(new DDSMipFace
                        {
                            FaceIndex = face,
                            MipIndex = mip,
                            Width = w,
                            Height = h,
                            Data = data
                        });

                        w = Math.Max(1, w / 2);
                        h = Math.Max(1, h / 2);
                    }
                }

                return new DDSFile
                {
                    Width = width,
                    Height = height,
                    MipCount = mipCount,
                    Format = format,
                    IsCubemap = isCubemap,
                    Faces = mipFaces
                };
            }
        }
    }
}