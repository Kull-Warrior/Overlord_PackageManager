using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class ObjectBoneArrayEntry(uint id, uint relOffset) : ValueEntry<ObjectBone[]>(id, relOffset)
    {
        private const int ObjectBoneArraySizeInBytes = 
            32 // Name (char[32] always 32 Bytes)
            + (24 * sizeof(float))  // Transform (Matrix4x4 + Quaternion + Translation)
            + (4 * sizeof(int));  // Integer-Indizes     (Self, Parent, Sibling, Traversal, Reserved)

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            // Calculate object bone count based on ObjectBoneArraySizeInBytes per object bone
            Value = new ObjectBone[PayloadLength / ObjectBoneArraySizeInBytes];

            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = new ObjectBone(
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
        }

        public override long GetPayloadSize()
        {
            return (long)Value.Length * ObjectBoneArraySizeInBytes;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            for (int i = 0; i < Value.Length; i++)
            {
                ObjectBone bone = Value[i];

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
}