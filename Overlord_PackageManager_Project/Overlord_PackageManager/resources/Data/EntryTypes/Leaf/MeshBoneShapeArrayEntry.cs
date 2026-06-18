using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public readonly record struct Matrix3x3(float M11, float M12, float M13,float M21, float M22, float M23,float M31, float M32, float M33)
    {
        // Statische Eigenschaften analog zu Matrix4x4
        public static Matrix3x3 Identity => new(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);

        public bool IsIdentity =>
            M11 == 1f && M12 == 0f && M13 == 0f &&
            M21 == 0f && M22 == 1f && M23 == 0f &&
            M31 == 0f && M32 == 0f && M33 == 1f;

        // Mathematische Operatoren (+, -, *)
        public static Matrix3x3 operator +(Matrix3x3 m1, Matrix3x3 m2) => new(
            m1.M11 + m2.M11, m1.M12 + m2.M12, m1.M13 + m2.M13,
            m1.M21 + m2.M21, m1.M22 + m2.M22, m1.M23 + m2.M23,
            m1.M31 + m2.M31, m1.M32 + m2.M32, m1.M33 + m2.M33
        );

        public static Matrix3x3 operator -(Matrix3x3 m1, Matrix3x3 m2) => new(
            m1.M11 - m2.M11, m1.M12 - m2.M12, m1.M13 - m2.M13,
            m1.M21 - m2.M21, m1.M22 - m2.M22, m1.M23 - m2.M23,
            m1.M31 - m2.M31, m1.M32 - m2.M32, m1.M33 - m2.M33
        );

        // Matrix-Multiplikation
        public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2) => new(
            m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31,
            m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32,
            m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33,

            m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31,
            m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32,
            m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33,

            m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31,
            m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32,
            m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33
        );

        // Skalar-Multiplikation
        public static Matrix3x3 operator *(Matrix3x3 matrix, float scalar) => new(
            matrix.M11 * scalar, matrix.M12 * scalar, matrix.M13 * scalar,
            matrix.M21 * scalar, matrix.M22 * scalar, matrix.M23 * scalar,
            matrix.M31 * scalar, matrix.M32 * scalar, matrix.M33 * scalar
        );

        // Standard-Methoden aus Matrix4x4
        public float GetDeterminant() =>
            M11 * (M22 * M33 - M23 * M32) -
            M12 * (M21 * M33 - M23 * M31) +
            M13 * (M21 * M32 - M22 * M31);

        public static Matrix3x3 Transpose(Matrix3x3 matrix) => new(
            matrix.M11, matrix.M21, matrix.M31,
            matrix.M12, matrix.M22, matrix.M32,
            matrix.M13, matrix.M23, matrix.M33
        );
    }

    public record MeshBoneShape(Matrix3x3 Matrix, Vector3 Head, Vector3 Tail);

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