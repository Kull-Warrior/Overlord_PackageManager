using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class Int32Entry(uint id, uint relOffset) : ScalarEntry<int>(id, relOffset)
    {
        protected override int ElementSize => sizeof(int);

        protected override int ReadValue(BinaryReader reader) => reader.ReadInt32();

        protected override void WriteValue(BinaryWriter writer, int value) => writer.Write(value);
    }
}