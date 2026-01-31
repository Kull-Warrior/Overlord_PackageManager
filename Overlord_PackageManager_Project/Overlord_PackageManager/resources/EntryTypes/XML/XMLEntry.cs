using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.XML
{
    class XMLEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varRefTable = new RefTable(reader, entryFactory);

            if (varRefTable.Count8 > 0 || varRefTable.Count32 > 0)
            {
                foreach (var entry in varRefTable.Entries)
                {
                    if (entry is StringEntry || entry is Int32Entry)
                    {
                        entry.Read(reader, varRefTable.origin);
                    }
                    if (entry is BinaryEntry)
                    {
                        Int32Entry? intEntry = varRefTable.Entries.OfType<Int32Entry>().LastOrDefault();
                        if (intEntry == null)
                            throw new InvalidOperationException("No XML length found");

                        ((BinaryEntry)entry).Read(reader, varRefTable.origin, intEntry.varInt);
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
            if (varRefTable.Count8 > 0 || varRefTable.Count32 > 0)
            {
                string resourcePath = ((StringEntry)varRefTable.Entries[0]).varString;
                string fileName = Path.GetFileName(resourcePath);

                byte[] data = ((BinaryEntry)varRefTable.Entries[2]).varBytes;

                using FileStream fileHeaderStream = File.Open(baseDir + "\\" + fileName, FileMode.Create);
                using BinaryWriter fileHeaderBinaryWriter = new BinaryWriter(fileHeaderStream);
                {
                    fileHeaderBinaryWriter.Write(data);
                }
            }
        }
    }
}
