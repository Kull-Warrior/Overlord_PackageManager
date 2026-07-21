using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class Int16ArrayEntry(uint id, uint relOffset) : RawArrayEntry<short>(id, relOffset)
    {
        protected override int ElementSize => sizeof(short);

        protected override short ReadValue(BinaryReader reader) => reader.ReadInt16();

        protected override void WriteValue(BinaryWriter writer, short value) => writer.Write(value);
    }
}