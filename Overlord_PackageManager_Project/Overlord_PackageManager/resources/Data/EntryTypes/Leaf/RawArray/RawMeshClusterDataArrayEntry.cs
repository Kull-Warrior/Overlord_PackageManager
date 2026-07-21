using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class RawMeshClusterDataArrayEntry(uint id, uint relOffset) : ValueEntry<RawMeshClusterData[]>(id, relOffset)
    {
        private const int RawMeshClusterDataSizeInBytes = (9 + 3 + 3) * sizeof(float) + sizeof(ushort) + sizeof(ushort); // 64 bytes

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            Value = new RawMeshClusterData[PayloadLength / RawMeshClusterDataSizeInBytes];

            for (int i = 0; i < Value.Length; i++)
            {
                Matrix3x3 matrix = new(
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
                );

                Vector3 center = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );

                Vector3 extents = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );

                Value[i] = new RawMeshClusterData(matrix, center, extents, reader.ReadUInt16(), reader.ReadUInt16());
            }
        }

        public override long GetPayloadSize()
        {
            return (long)Value.Length * RawMeshClusterDataSizeInBytes;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (RawMeshClusterData clusterData in Value)
            {
                Matrix3x3 matrix = clusterData.Matrix;

                // Matrix
                writer.Write(matrix.M11); writer.Write(matrix.M12); writer.Write(matrix.M13);
                writer.Write(matrix.M21); writer.Write(matrix.M22); writer.Write(matrix.M23);
                writer.Write(matrix.M31); writer.Write(matrix.M32); writer.Write(matrix.M33);

                // Head
                writer.Write(clusterData.Center.X);
                writer.Write(clusterData.Center.Y);
                writer.Write(clusterData.Center.Z);

                // Tail
                writer.Write(clusterData.Extents.X);
                writer.Write(clusterData.Extents.Y);
                writer.Write(clusterData.Extents.Z);

                // Patch Index and Triangle Count
                writer.Write(clusterData.patchIndex);
                writer.Write(clusterData.triangleCount);
            }
        }
    }
}