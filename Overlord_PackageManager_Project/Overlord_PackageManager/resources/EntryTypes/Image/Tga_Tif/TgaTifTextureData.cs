using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif
{
    class TgaTifTextureData(uint id, uint relOffset) : Entry(id, relOffset), IHasRefTable
    {
        public RefTable Table;
        public RefTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory, uint rawTextureDataLength)
        {
            reader.BaseStream.Position = origin + RelOffset;
            Table = new RefTable(reader, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is BinaryEntry)
                {
                    ((BinaryEntry)entry).Read(reader, Table.origin, rawTextureDataLength);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
