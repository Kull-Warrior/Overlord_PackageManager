using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public class MatricesArrayEntry(uint id, uint relOffset) : ValueEntry<Matrix4x4[]>(id, relOffset)
    {
        private const int MatrixSizeInBytes = 16 * sizeof(float); // 64 bytes

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            // Calculate matrix count based on 64 bytes per matrix
            Value = new Matrix4x4[PayloadLength / MatrixSizeInBytes];

            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = new Matrix4x4(
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
                );
            }
        }

        public override long GetPayloadSize()
        {
            return (long)Value.Length * MatrixSizeInBytes;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            for (int i = 0; i < Value.Length; i++)
            {
                Matrix4x4 matrix = Value[i];

                // Write all 16 float values sequentially (Row-Major order)
                writer.Write(matrix.M11); writer.Write(matrix.M12); writer.Write(matrix.M13); writer.Write(matrix.M14);
                writer.Write(matrix.M21); writer.Write(matrix.M22); writer.Write(matrix.M23); writer.Write(matrix.M24);
                writer.Write(matrix.M31); writer.Write(matrix.M32); writer.Write(matrix.M33); writer.Write(matrix.M34);
                writer.Write(matrix.M41); writer.Write(matrix.M42); writer.Write(matrix.M43); writer.Write(matrix.M44);
            }
        }
    }
}