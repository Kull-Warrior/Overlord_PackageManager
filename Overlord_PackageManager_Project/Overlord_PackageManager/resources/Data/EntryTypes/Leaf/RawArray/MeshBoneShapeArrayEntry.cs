using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class MeshBoneShapeArrayEntry(uint id, uint relOffset) : RawArrayEntry<MeshBoneShape>(id, relOffset)
    {
        protected override int ElementSize => (9 + 3 + 3) * sizeof(float); // 60 bytes

        protected override MeshBoneShape ReadValue(BinaryReader reader)
        {
            return new MeshBoneShape(
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
                   )
               );
        }

        protected override void WriteValue(BinaryWriter writer, MeshBoneShape boneShape)
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