using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class UInt32ArrayEntry(uint id, uint relOffset) : RawArrayEntry<uint>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override uint ReadValue(BinaryReader reader) => reader.ReadUInt32();

        protected override void WriteValue(BinaryWriter writer, uint value) => writer.Write(value);
    }
}