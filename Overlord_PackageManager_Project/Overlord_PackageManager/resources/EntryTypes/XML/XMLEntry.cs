using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.XML
{
    class XMLEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            reader.BaseStream.Position = origin + RelativeOffset;
            Table = new ReferenceTable(reader, end, entryFactory);

            if (Table.SmallEntryCount > 0 || Table.LargeEntryCount > 0)
            {
                foreach (var entry in Table.Entries)
                {
                    entry.Read(reader, Table.PayloadStartOffset);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public void WriteToFile(string baseDir)
        {
            if (Table.SmallEntryCount > 0 || Table.LargeEntryCount > 0)
            {
                string resourcePath = ((StringEntry)Table.Entries[0]).Value;
                string fileName = Path.GetFileName(resourcePath);

                byte[] data = ((BlobEntry)Table.Entries[2]).Value;

                using FileStream fileHeaderStream = File.Open(baseDir + "\\" + fileName, FileMode.Create);
                using BinaryWriter fileHeaderBinaryWriter = new BinaryWriter(fileHeaderStream);
                {
                    fileHeaderBinaryWriter.Write(data);
                }
            }
        }
    }
}
