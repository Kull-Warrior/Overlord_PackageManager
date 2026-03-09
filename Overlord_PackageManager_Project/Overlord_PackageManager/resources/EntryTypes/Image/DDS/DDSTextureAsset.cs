using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public sealed class DDSTextureAsset(uint id, uint relOffset) : DDSImageAssetBase(id, relOffset)
    {
        public override void ReplaceFromDDS(byte[] fileBytes)
        {
            DDSFile dds = DDSImageReader.Read(fileBytes);
            AssetList list = GetTextureList();
            list.Table.Entries.Clear();

            uint currentOffset = 0;
            for (int i = 0; i < dds.Faces.Count; i++)
            {
                DDSMipFace face = dds.Faces[i];
                DDSTextures tex = new DDSTextures((uint)i, currentOffset, face.Width, face.Height, dds.Format, face.Data);
                list.Table.Entries.Add(tex);

                currentOffset += 21 + (uint)face.Data.Length;
            }
        }

        public override void WriteToDDS(Stream output)
        {
            List<DDSTextures> textures = GetTextureList().Table.Entries.OfType<DDSTextures>().ToList();

            DDSTextures first = textures.First();
            uint width = ((Int32Entry)first.Table.Entries[0]).Value;
            uint height = ((Int32Entry)first.Table.Entries[1]).Value;
            DDSFormat format = (DDSFormat)((Int32Entry)first.Table.Entries[2]).Value;

            List<DDSMipFace> faces = textures.Select((t, i) =>
            {
                BlobEntry blob = t.Table.Entries.OfType<BlobEntry>().First();
                return new DDSMipFace
                {
                    FaceIndex = 0,
                    MipIndex = i,
                    Width = width >> i,
                    Height = height >> i,
                    Data = blob.Value
                };
            }).ToList();

            DDSFile file = new DDSFile
            {
                Width = width,
                Height = height,
                MipCount = (uint)faces.Count,
                Format = format,
                IsCubemap = false,
                Faces = faces
            };

            DDSImageWriter.Write(output, file);
        }
    }
}