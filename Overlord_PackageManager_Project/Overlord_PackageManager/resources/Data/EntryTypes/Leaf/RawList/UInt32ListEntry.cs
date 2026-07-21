using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class UInt32ListEntry(uint id, uint relOffset) : RawListEntry<uint>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override uint ReadValue(BinaryReader reader) => reader.ReadUInt32();

        protected override void WriteValue(BinaryWriter writer, uint value) => writer.Write(value);
    }
}