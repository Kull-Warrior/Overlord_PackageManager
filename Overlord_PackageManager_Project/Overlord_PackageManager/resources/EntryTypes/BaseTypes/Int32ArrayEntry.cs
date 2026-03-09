using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    public class Int32ArrayEntry(uint id, uint relOffset) : ValueEntry<uint[]>(id, relOffset)
    {
        public uint NumberOfValues;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            NumberOfValues = reader.ReadUInt32();
            Value = new uint[NumberOfValues];
            
            for (int i = 0; i < NumberOfValues; i++)
            {
                Value[i] = reader.ReadUInt32();
            }
        }
    }
}
