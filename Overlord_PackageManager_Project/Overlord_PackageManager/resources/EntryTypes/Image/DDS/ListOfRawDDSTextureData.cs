using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    class ListOfRawDDSTextureData(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes(3);
            Table = new ReferenceTable(reader);

            foreach (var entry in Table.Entries)
            {
                if (entry is RawDDSTextureData)
                {
                    ((RawDDSTextureData)entry).Read(reader, Table.OffsetOrigin, 0, RawDDSTextureDataDictionary);
                }
            }
        }
    }
}
