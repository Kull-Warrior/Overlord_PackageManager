using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class RawMeshClusterDataArrayEntry(uint id, uint relOffset) : RawArrayEntry<RawMeshClusterData>(id, relOffset)
    {
        protected override int ElementSize => (9 + 3 + 3) * sizeof(float) + sizeof(ushort) + sizeof(ushort);

        protected override RawMeshClusterData ReadValue(BinaryReader reader)
        {
            return new RawMeshClusterData(
                    new Matrix3x3(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
                    ),
                    new Vector3(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()
                    ),
                    new Vector3(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()
                    ),
                    reader.ReadUInt16(),
                    reader.ReadUInt16()
                );
        }

        protected override void WriteValue(BinaryWriter writer, RawMeshClusterData value)
        {
            Matrix3x3 matrix = value.Matrix;

            // Matrix
            writer.Write(matrix.M11); writer.Write(matrix.M12); writer.Write(matrix.M13);
            writer.Write(matrix.M21); writer.Write(matrix.M22); writer.Write(matrix.M23);
            writer.Write(matrix.M31); writer.Write(matrix.M32); writer.Write(matrix.M33);

            // Head
            writer.Write(value.Center.X);
            writer.Write(value.Center.Y);
            writer.Write(value.Center.Z);

            // Tail
            writer.Write(value.Extents.X);
            writer.Write(value.Extents.Y);
            writer.Write(value.Extents.Z);

            // Patch Index and Triangle Count
            writer.Write(value.patchIndex);
            writer.Write(value.triangleCount);
        }
    }
}