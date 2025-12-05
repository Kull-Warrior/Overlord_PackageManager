using System.IO;

namespace Overlord_PackageManager.resources.Generic.EntryTypes
{
    class RPKEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public uint unknown;
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            unknown = reader.ReadUInt32();
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                entry.Read(reader, varRefTable.origin);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
