using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class FloatListEntry(uint id, uint relOffset) : RawListEntry<float>(id, relOffset)
    {
        protected override int ElementSize => sizeof(float);

        protected override float ReadValue(BinaryReader reader) => reader.ReadSingle();

        protected override void WriteValue(BinaryWriter writer, float value) => writer.Write(value);
    }
}