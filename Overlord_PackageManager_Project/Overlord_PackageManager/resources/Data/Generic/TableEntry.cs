using Overlord_PackageManager.resources.Data.Interfaces;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Generic
{
    public class TableEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table { get; set; }

        public ReferenceTable GetReferenceTable() => Table;

        // Grammar definition for this table type
        protected virtual Func<BinaryReader, uint, uint, Entry> EntryFactory => null;

        protected virtual int PayloadOffset => 0;

        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start + PayloadOffset;
            Table = new ReferenceTable();
            Table.TableEndOffset = end;
            Table.ReadHeader(reader);
            Table.ReadEntryStructure(reader, EntryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
            }
        }

        public override long GetPayloadSize()
        {
            return PayloadOffset + Table.ComputeLayout();
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            long start = origin + RelativeOffset;
            long tableStart = start + PayloadOffset;
            writer.BaseStream.Position = tableStart;
            Table.Write(writer, tableStart);
        }
    }
}