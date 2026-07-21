using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class ObjectBoneArrayEntry(uint id, uint relOffset) : RawArrayEntry<ObjectBone>(id, relOffset)
    {
        // The size of an ObjectBone is 32 bytes for the name, 24 floats for the transform, and 4 integers for the indices.
        protected override int ElementSize => 32 + (24 * sizeof(float)) + (4 * sizeof(int));

        protected override ObjectBone ReadValue(BinaryReader reader)
        {
            return new ObjectBone(
                    reader.ReadChars(32),
                    new Transform(
                        new Matrix4x4(
                            reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                            reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                            reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                            reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
                        ),
                        new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
                        new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
                    ),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32()
                );
        }

        protected override void WriteValue(BinaryWriter writer, ObjectBone bone)
        {
            writer.Write(bone.Name);

            writer.Write(bone.Transform.Matrix.M11); writer.Write(bone.Transform.Matrix.M12); writer.Write(bone.Transform.Matrix.M13); writer.Write(bone.Transform.Matrix.M14);
            writer.Write(bone.Transform.Matrix.M21); writer.Write(bone.Transform.Matrix.M22); writer.Write(bone.Transform.Matrix.M23); writer.Write(bone.Transform.Matrix.M24);
            writer.Write(bone.Transform.Matrix.M31); writer.Write(bone.Transform.Matrix.M32); writer.Write(bone.Transform.Matrix.M33); writer.Write(bone.Transform.Matrix.M34);
            writer.Write(bone.Transform.Matrix.M41); writer.Write(bone.Transform.Matrix.M42); writer.Write(bone.Transform.Matrix.M43); writer.Write(bone.Transform.Matrix.M44);
            writer.Write(bone.Transform.Rotation.X); writer.Write(bone.Transform.Rotation.Y); writer.Write(bone.Transform.Rotation.Z); writer.Write(bone.Transform.Rotation.W);
            writer.Write(bone.Transform.Translation.X); writer.Write(bone.Transform.Translation.Y); writer.Write(bone.Transform.Translation.Z); writer.Write(bone.Transform.Translation.W);

            writer.Write(bone.ParentIndex);
            writer.Write(bone.NextSiblingIndex);
            writer.Write(bone.NextTraversalIndex);
            writer.Write(bone.Reserved);
        }
    }
}