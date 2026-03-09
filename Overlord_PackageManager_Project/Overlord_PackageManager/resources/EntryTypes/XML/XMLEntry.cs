using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.XML
{
    class XMLEntry(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => Entry.XMLDictionary;

        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end, EntryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
            }
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
