using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class QuaternionListEntry(uint id, uint relOffset) : RawListEntry<Quaternion>(id, relOffset)
    {
        protected override int ElementSize => 4 * sizeof(float);

        protected override Quaternion ReadValue(BinaryReader reader)
        {
            return new Quaternion(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, Quaternion value)
        {
            writer.Write(value.X); writer.Write(value.Y); writer.Write(value.Z); writer.Write(value.W);
        }
    }
}