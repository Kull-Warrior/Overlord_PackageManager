using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class UInt32CountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<uint>(id, relOffset)
    {
        protected override uint ReadValue(BinaryReader reader) => reader.ReadUInt32();

        protected override void WriteValue(BinaryWriter writer, uint value) => writer.Write(value);

        protected override int ElementSize => sizeof(uint);
    }
}
