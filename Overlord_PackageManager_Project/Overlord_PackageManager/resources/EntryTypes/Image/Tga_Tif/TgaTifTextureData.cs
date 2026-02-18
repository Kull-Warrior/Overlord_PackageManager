using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif
{
    class TgaTifTextureData(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory, uint rawTextureDataLength)
        {
            reader.BaseStream.Position = origin + RelOffset;
            Table = new ReferenceTable(reader, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is BlobEntry)
                {
                    ((BlobEntry)entry).Read(reader, Table.OffsetOrigin, rawTextureDataLength);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
