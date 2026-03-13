using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Files.DDS;
using System.IO;
using Overlord_PackageManager.resources.Data.Factories;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS
{
    public class DDSTextures : AssetEntry
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => DDSTextureFactory.CreateDDSTexture;

        public DDSTextures(uint id, uint relOffset, uint typeIdentifier) : base(id, relOffset, typeIdentifier)
        {
        }

        public DDSTextures(uint id, uint relOffset, uint typeIdentifier, uint width, uint height, DDSFormat format, byte[] data)
            : base(id, relOffset, typeIdentifier)
        {
            Id = id;
            RelativeOffset = relOffset;
            TypeIdentifier = typeIdentifier;
            PayloadLength = 12 + (uint)data.Length;
            Table = new ReferenceTable();
            Table.Entries = new List<Entry>(4);
            Table.Entries.Add(new Int32Entry(20, 0) { Value = width });
            Table.Entries.Add(new Int32Entry(21, 4) { Value = height });
            Table.Entries.Add(new Int32Entry(23, 8) { Value = (uint)format });
            Table.Entries.Add(new BlobEntry(22, 12) { Value = data });
        }
    }
}
