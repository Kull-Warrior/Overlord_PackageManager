using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public sealed class ReflectionCubeMapAsset : DDSImageAssetBase
    {
        public ReflectionCubeMapAsset(uint id, uint relOffset) : base(id, relOffset) { }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public override void ReplaceFromDDS(byte[] fileBytes)
        {
            DDSFile dds = DDSImageReader.Read(fileBytes);
            AssetList list = GetTextureList();
            list.Table.Entries.Clear();

            foreach (DDSMipFace face in dds.Faces)
            {
                DDSTextures tex = new DDSTextures(face.Width, face.Height, dds.Format, face.Data);
                list.Table.Entries.Add(tex);
            }
        }

        public override void WriteToDDS(Stream output)
        {
            List<DDSTextures> textures = GetTextureList().Table.Entries.OfType<DDSTextures>().ToList();
            DDSTextures first = textures.First();

            uint width = ((Int32Entry)first.Table.Entries[0]).Value;
            uint height = ((Int32Entry)first.Table.Entries[1]).Value;
            DDSFormat format = (DDSFormat)((Int32Entry)first.Table.Entries[2]).Value;

            int mipCount = textures.Count / 6;

            List<DDSMipFace> faces = textures.Select((t, i) =>
            {
                BlobEntry blob = t.Table.Entries.OfType<BlobEntry>().First();

                return new DDSMipFace
                {
                    FaceIndex = i / mipCount,
                    MipIndex = i % mipCount,
                    Width = width >> (i % mipCount),
                    Height = height >> (i % mipCount),
                    Data = blob.Value
                };
            }).ToList();

            DDSFile file = new DDSFile
            {
                Width = width,
                Height = height,
                MipCount = (uint)mipCount,
                Format = format,
                IsCubemap = true,
                Faces = faces
            };

            DDSImageWriter.Write(output, file);
        }
    }
}