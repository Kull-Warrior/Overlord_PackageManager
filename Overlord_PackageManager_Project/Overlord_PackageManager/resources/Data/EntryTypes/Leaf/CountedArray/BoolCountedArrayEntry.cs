using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class BoolCountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<bool>(id, relOffset)
    {
        protected override int ElementSize => sizeof(byte);

        protected override bool ReadValue(BinaryReader reader) => reader.ReadBoolean();

        protected override void WriteValue(BinaryWriter writer, bool value) => writer.Write(value);
    }
}