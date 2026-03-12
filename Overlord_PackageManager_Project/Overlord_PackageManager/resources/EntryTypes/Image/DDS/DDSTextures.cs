using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public class DDSTextures : AssetEntry
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => Entry.DDSTextureDictionary;

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
