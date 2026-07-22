using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class Int32CountedListEntry(uint id, uint relOffset) : CountedListEntry<int>(id, relOffset)
    {
        protected override int ElementSize => sizeof(int);

        protected override int ReadValue(BinaryReader reader) => reader.ReadInt32();

        protected override void WriteValue(BinaryWriter writer, int value) => writer.Write(value);
    }
}