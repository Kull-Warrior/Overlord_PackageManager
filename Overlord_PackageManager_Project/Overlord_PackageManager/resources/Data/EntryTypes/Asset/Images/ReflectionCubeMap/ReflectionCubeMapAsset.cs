using Overlord_PackageManager.resources.Data.Files.DDS;
using System.IO;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.ReflectionCubeMap
{
    public sealed class ReflectionCubeMapAsset : DDSImageAssetBase
    {
        public ReflectionCubeMapAsset(uint id, uint relOffset, uint typeIdentifier) : base(id, relOffset, typeIdentifier) { }

        public override void ReplaceFromDDS(byte[] fileBytes)
        {
            DDSFile dds = DDSImageReader.Read(fileBytes);
            AssetList list = GetTextureList();
            list.Table.Entries.Clear();

            uint currentOffset = 0;
            for (int i = 0; i < dds.Faces.Count; i++)
            {
                DDSMipFace face = dds.Faces[i];
                DDSTextures tex = new DDSTextures((uint)i, currentOffset, 4259876, face.Width, face.Height, dds.Format, face.Data);
                list.Table.Entries.Add(tex);

                currentOffset += 21 + (uint)face.Data.Length;
            }
        }

        public override void WriteToDDS(Stream output)
        {
            List<DDSTextures> textures = GetTextureList().Table.Entries.OfType<DDSTextures>().ToList();
            DDSTextures first = textures.First();

            uint width = ((UInt32Entry)first.Table.Entries[0]).Value;
            uint height = ((UInt32Entry)first.Table.Entries[1]).Value;
            DDSFormat format = (DDSFormat)((UInt32Entry)first.Table.Entries[2]).Value;

            int mipCount = textures.Count / 6;

            List<DDSMipFace> faces = textures.Select((t, i) =>
            {
                ByteArrayEntry blob = t.Table.Entries.OfType<ByteArrayEntry>().First();

                return new DDSMipFace
                {
                    FaceIndex = i / mipCount,
                    MipIndex = i % mipCount,
                    Width = width >> i % mipCount,
                    Height = height >> i % mipCount,
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