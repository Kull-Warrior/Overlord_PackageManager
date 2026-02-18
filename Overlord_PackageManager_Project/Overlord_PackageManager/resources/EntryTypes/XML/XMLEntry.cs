using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.XML
{
    class XMLEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            Table = new ReferenceTable(reader, entryFactory);

            if (Table.Count8 > 0 || Table.Count32 > 0)
            {
                foreach (var entry in Table.Entries)
                {
                    if (entry is StringEntry || entry is Int32Entry)
                    {
                        entry.Read(reader, Table.OffsetOrigin);
                    }
                    if (entry is BinaryEntry)
                    {
                        Int32Entry? intEntry = Table.Entries.OfType<Int32Entry>().LastOrDefault();
                        if (intEntry == null)
                            throw new InvalidOperationException("No XML length found");

                        ((BinaryEntry)entry).Read(reader, Table.OffsetOrigin, intEntry.varInt);
                    }
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public void WriteToFile(string baseDir)
        {
            if (Table.Count8 > 0 || Table.Count32 > 0)
            {
                string resourcePath = ((StringEntry)Table.Entries[0]).varString;
                string fileName = Path.GetFileName(resourcePath);

                byte[] data = ((BinaryEntry)Table.Entries[2]).varBytes;

                using FileStream fileHeaderStream = File.Open(baseDir + "\\" + fileName, FileMode.Create);
                using BinaryWriter fileHeaderBinaryWriter = new BinaryWriter(fileHeaderStream);
                {
                    fileHeaderBinaryWriter.Write(data);
                }
            }
        }
    }
}
