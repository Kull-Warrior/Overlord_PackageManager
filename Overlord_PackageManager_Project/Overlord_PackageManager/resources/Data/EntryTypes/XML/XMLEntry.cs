using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.IO;
using Overlord_PackageManager.resources.Data.Factories;

namespace Overlord_PackageManager.resources.Data.EntryTypes.XML
{
    public class XMLEntry(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => XMLFactory.CreateXML;

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
