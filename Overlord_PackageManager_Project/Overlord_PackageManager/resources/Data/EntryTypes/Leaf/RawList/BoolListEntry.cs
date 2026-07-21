using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class BoolListEntry(uint id, uint relOffset) : RawListEntry<bool>(id, relOffset)
    {
        protected override int ElementSize => sizeof(byte);

        protected override bool ReadValue(BinaryReader reader) => reader.ReadBoolean();

        protected override void WriteValue(BinaryWriter writer, bool value) => writer.Write(value);
    }
}