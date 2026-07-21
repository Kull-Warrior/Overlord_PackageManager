using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class Int32ArrayEntry(uint id, uint relOffset) : RawArrayEntry<int>(id, relOffset)
    {
        protected override int ElementSize => sizeof(int);

        protected override int ReadValue(BinaryReader reader) => reader.ReadInt32();

        protected override void WriteValue(BinaryWriter writer, int value) => writer.Write(value);
    }
}