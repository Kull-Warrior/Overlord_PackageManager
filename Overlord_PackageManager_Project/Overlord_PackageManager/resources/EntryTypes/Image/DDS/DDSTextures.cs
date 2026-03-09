using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public class DDSTextures : AssetEntry
    {
        public DDSTextures(uint id, uint relOffset) : base(id, relOffset)
        {
        }

        public DDSTextures(uint id, uint relOffset, uint width, uint height, DDSFormat format, byte[] data)
            : base(id, relOffset)
        {
            Table = new ReferenceTable();
            Table.Entries.Add(new Int32Entry(20, 0) { Value = width });
            Table.Entries.Add(new Int32Entry(21, 4) { Value = height });
            Table.Entries.Add(new Int32Entry(23, 8) { Value = (uint)format });
            Table.Entries.Add(new BlobEntry(22, 12) { Value = data });
        }


        public void Read(BinaryReader reader, long origin, Func<BinaryReader, uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
