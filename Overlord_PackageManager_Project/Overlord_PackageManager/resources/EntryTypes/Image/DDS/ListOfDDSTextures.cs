using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public class ListOfDDSTextures : Entry, IHasReferenceTable
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public ListOfDDSTextures()
        {

        }
        public ListOfDDSTextures(uint id, uint relOffset) : base(id, relOffset)
        {

        }

        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelOffset;
            long end = start + Length;

            reader.BaseStream.Position = start;
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes(3);
            Table = new ReferenceTable(reader, end);

            foreach (var entry in Table.Entries)
            {
                if (entry is DDSTextures)
                {
                    ((DDSTextures)entry).Read(reader, Table.OffsetOrigin, DDSTextureDictionary);
                }
            }
        }
    }
}
