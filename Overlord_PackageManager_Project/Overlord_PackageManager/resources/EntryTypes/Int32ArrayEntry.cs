using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class Int32ArrayEntry : Entry
    {
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public uint[] varIntArray;

        public Int32ArrayEntry(uint id, uint relOffset) : base (id, relOffset)
        {

        }

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
