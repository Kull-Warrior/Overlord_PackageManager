using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public class DDSTextures : Entry, IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public DDSTextures(uint id, uint relOffset) : base(id, relOffset)
        {

        }

        public DDSTextures (uint width, uint height, DDSFormat format, byte[] data)
        {
            Table = new ReferenceTable();
            Table.Entries.Add(new Int32Entry(20, 0) { varInt = width });
            Table.Entries.Add(new Int32Entry(21, 4) { varInt = height });
            Table.Entries.Add(new Int32Entry(23, 8) { varInt = (uint)format });
            Table.Entries.Add(new BlobEntry(22, 12) { varBytes = data });
        }


        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            reader.BaseStream.Position = origin + RelativeOffset;
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
