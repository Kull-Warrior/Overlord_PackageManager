using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class FloatEntry : Entry
    {
        public float varFloat;

        public FloatEntry(uint id, uint relOffset) : base(id, relOffset)
        {

        }
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varFloat = reader.ReadSingle();
        }
    }
}
