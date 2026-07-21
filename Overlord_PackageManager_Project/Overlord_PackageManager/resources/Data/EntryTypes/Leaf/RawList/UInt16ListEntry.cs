using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class UInt16ListEntry(uint id, uint relOffset) : RawListEntry<ushort>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ushort);

        protected override ushort ReadValue(BinaryReader reader) => reader.ReadUInt16();

        protected override void WriteValue(BinaryWriter writer, ushort value) => writer.Write(value);
    }
}