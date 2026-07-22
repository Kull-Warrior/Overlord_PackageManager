using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class Int16CountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<short>(id, relOffset)
    {
        protected override int ElementSize => sizeof(short);

        protected override short ReadValue(BinaryReader reader) => reader.ReadInt16();

        protected override void WriteValue(BinaryWriter writer, short value) => writer.Write(value);
    }
}