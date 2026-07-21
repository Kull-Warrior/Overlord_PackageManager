using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class BoolEntry(uint id, uint relOffset) : ScalarEntry<bool>(id, relOffset)
    {
        protected override int ElementSize => sizeof(bool);

        protected override bool ReadValue(BinaryReader reader) => reader.ReadBoolean();

        protected override void WriteValue(BinaryWriter writer, bool value) => writer.Write(value);
    }
}