using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class MeshBoneShapeArrayEntry(uint id, uint relOffset) : ValueEntry<MeshBoneShape[]>(id, relOffset)
    {
        private const int MeshBoneShapeSizeInBytes = (9 + 3 + 3) * sizeof(float); // 60 bytes

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            Value = new MeshBoneShape[PayloadLength / MeshBoneShapeSizeInBytes];

            for (int i = 0; i < Value.Length; i++)
            {
                Matrix3x3 matrix = new(
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
                );

                Vector3 head = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );

                Vector3 tail = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );

                Value[i] = new MeshBoneShape(matrix, head, tail);
            }
        }

        public override long GetPayloadSize()
        {
            return (long)Value.Length * MeshBoneShapeSizeInBytes;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (MeshBoneShape boneShape in Value)
            {
                Matrix3x3 matrix = boneShape.Matrix;

                // Matrix
                writer.Write(matrix.M11); writer.Write(matrix.M12); writer.Write(matrix.M13);
                writer.Write(matrix.M21); writer.Write(matrix.M22); writer.Write(matrix.M23);
                writer.Write(matrix.M31); writer.Write(matrix.M32); writer.Write(matrix.M33);

                // Head
                writer.Write(boneShape.Head.X);
                writer.Write(boneShape.Head.Y);
                writer.Write(boneShape.Head.Z);

                // Tail
                writer.Write(boneShape.Tail.X);
                writer.Write(boneShape.Tail.Y);
                writer.Write(boneShape.Tail.Z);
            }
        }
    }
}