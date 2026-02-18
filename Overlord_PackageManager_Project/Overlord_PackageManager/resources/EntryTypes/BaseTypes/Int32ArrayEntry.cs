using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class Int32ArrayEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasRefTable
    {
        public byte[] leadingBytes;
        public RefTable Table;

        public uint[] varIntArray;

        public RefTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, uint arraySize)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varIntArray = new uint[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                varIntArray[i] = reader.ReadUInt32();
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {

            throw new NotImplementedException();
        }
    }
}
