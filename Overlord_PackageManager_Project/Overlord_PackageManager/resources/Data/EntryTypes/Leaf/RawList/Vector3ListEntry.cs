using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class Vector3ListEntry(uint id, uint relOffset) : RawListEntry<Vector3>(id, relOffset)
    {
        protected override int ElementSize => 3 * sizeof(float);

        protected override Vector3 ReadValue(BinaryReader reader)
        {
            return new Vector3(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.X); writer.Write(value.Y); writer.Write(value.Z);
        }
    }
}