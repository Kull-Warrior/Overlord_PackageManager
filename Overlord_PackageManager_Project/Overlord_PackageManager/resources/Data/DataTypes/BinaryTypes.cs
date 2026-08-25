using System.Numerics;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public static class BinaryTypes
    {
        public static readonly BinaryType<byte> Byte =
        new()
        {
            Size = 1,
            DisplayName = "byte",
            Read = r => r.ReadByte(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<bool> Bool =
        new()
        {
            Size = 1,
            DisplayName = "bool",
            Read = r => r.ReadBoolean(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<char> Char =
        new()
        {
            Size = 1,
            DisplayName = "char",
            Read = r => r.ReadChar(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<short> Int16 =
        new()
        {
            Size = 2,
            DisplayName = "int16",
            Read = r => r.ReadInt16(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<ushort> UInt16 =
        new()
        {
            Size = 2,
            DisplayName = "uint16",
            Read = r => r.ReadUInt16(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<int> Int32 =
        new()
        {
            Size = 4,
            DisplayName = "int32",
            Read = r => r.ReadInt32(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<uint> UInt32 =
        new()
        {
            Size = 4,
            DisplayName = "uint32",
            Read = r => r.ReadUInt32(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<long> Int64 =
        new()
        {
            Size = 8,
            DisplayName = "int64",
            Read = r => r.ReadInt64(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<ulong> UInt64 =
        new()
        {
            Size = 8,
            DisplayName = "uint64",
            Read = r => r.ReadUInt64(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<float> Float =
        new()
        {
            Size = 4,
            DisplayName = "float",
            Read = r => r.ReadSingle(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<double> Double =
        new()
        {
            Size = 8,
            DisplayName = "double",
            Read = r => r.ReadDouble(),
            Write = (w, v) => w.Write(v)
        };

        public static readonly BinaryType<Matrix3x3> Matrix3x3 =
        new()
        {
            Size = 36, // 3x3 matrix of floats
            DisplayName = "Matrix3x3",
            Read = r =>
            {
                return new Matrix3x3(
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle()
                );
            },
            Write = (w, v) =>
            {
                w.Write(v.M11); w.Write(v.M12); w.Write(v.M13);
                w.Write(v.M21); w.Write(v.M22); w.Write(v.M23);
                w.Write(v.M31); w.Write(v.M32); w.Write(v.M33);
            }
        };

        public static readonly BinaryType<Matrix4x4> Matrix4x4 =
        new()
        {
            Size = 64, // 4x4 matrix of floats
            DisplayName = "Matrix4x4",
            Read = r =>
            {
                return new Matrix4x4(
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                    r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()
                );
            },
            Write = (w, v) =>
            {
                w.Write(v.M11); w.Write(v.M12); w.Write(v.M13); w.Write(v.M14);
                w.Write(v.M21); w.Write(v.M22); w.Write(v.M23); w.Write(v.M24);
                w.Write(v.M31); w.Write(v.M32); w.Write(v.M33); w.Write(v.M34);
                w.Write(v.M41); w.Write(v.M42); w.Write(v.M43); w.Write(v.M44);
            }
        };

        public static readonly BinaryType<Vector3> Vector3 =
        new()
        {
            Size = 12, // 3 floats
            DisplayName = "Vector3",
            Read = r =>
            {
                return new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
            },
            Write = (w, v) =>
            {
                w.Write(v.X);
                w.Write(v.Y);
                w.Write(v.Z);
            }
        };

        public static readonly BinaryType<Vector4> Vector4 =
        new()
        {
            Size = 16, // 4 floats
            DisplayName = "Vector4",
            Read = r =>
            {
                return new Vector4(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
            },
            Write = (w, v) =>
            {
                w.Write(v.X);
                w.Write(v.Y);
                w.Write(v.Z);
                w.Write(v.W);
            }
        };

        public static readonly BinaryType<Quaternion> Quaternion =
        new()
        {
            Size = 16, // 4 floats
            DisplayName = "Quaternion",
            Read = r =>
            {
                return new Quaternion(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
            },
            Write = (w, v) =>
            {
                w.Write(v.X);
                w.Write(v.Y);
                w.Write(v.Z);
                w.Write(v.W);
            }
        };

        public static readonly BinaryType<BonePosition> BonePosition =
        new()
        {
            Size =  16, //Timestamp (UInt32 / microseconds) + float X, float Y, float Z
            DisplayName = "Position",
            Read = r =>
            {
                return new BonePosition(r.ReadUInt32(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
            },
            Write = (w, v) =>
            {
                w.Write(v.Timestamp);
                w.Write(v.X);
                w.Write(v.Y);
                w.Write(v.Z);
            }
        };

        public static readonly BinaryType<BoneRotation> BoneRotation =
        new()
        {
            Size = 6, //float Pitch, float Yaw, float Roll
            DisplayName = "Rotation",
            Read = r =>
            {
                int rawX = r.ReadInt16();
                int rawY = r.ReadInt16();
                int rawZ = r.ReadInt16();

                // Convert to radians.
                // 32768 units = 180° (PI radians)
                float radiusX = rawX * (MathF.PI / 32768.0f);
                float radiusY = rawY * (MathF.PI / 32768.0f);
                float radiusZ = rawZ * (MathF.PI / 32768.0f);

                return new BoneRotation(radiusX, radiusY, radiusZ);
            },
            Write = (w, v) =>
            {
                const float scale = 32768.0f / MathF.PI;

                static short ToInt16(float rad)
                {
                    float val = rad * scale;

                    // Clamp to the valid signed 16-bit range
                    if (val > short.MaxValue)
                    {
                        val = short.MaxValue;
                    }
                    if (val < short.MinValue)
                    {
                        val = short.MinValue;
                    }

                    // Round to nearest integer
                    return (short)Math.Round(val, MidpointRounding.AwayFromZero);
                }

                w.Write(ToInt16(v.Pitch));
                w.Write(ToInt16(v.Yaw));
                w.Write(ToInt16(v.Roll));
            }
        };

        public static readonly BinaryType<BoneScale> BoneScale =
        new()
        {
            Size = 6, //half ScaleX, half ScaleY, half ScaleZ
            DisplayName = "Scale",
            Read = r =>
            {
                return new BoneScale(r.ReadHalf(), r.ReadHalf(), r.ReadHalf());
            },
            Write = (w, v) =>
            {
                w.Write(v.ScaleX);
                w.Write(v.ScaleY);
                w.Write(v.ScaleZ);
            }
        };

        public static readonly BinaryType<MeshBoneShape> MeshBoneShape =
        new()
        {
            Size = 60, //Matrix3x3 Matrix, Vector3 Head, Vector3 Tail);
            DisplayName = "MeshBoneShape",
            Read = r =>
            {
                return new MeshBoneShape(
                   new Matrix3x3(
                       r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                       r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                       r.ReadSingle(), r.ReadSingle(), r.ReadSingle()
                   ),

                   new Vector3(
                       r.ReadSingle(),
                       r.ReadSingle(),
                       r.ReadSingle()
                   ),

                   new Vector3(
                       r.ReadSingle(),
                       r.ReadSingle(),
                       r.ReadSingle()
                   )
               );
            },
            Write = (w, v) =>
            {
                Matrix3x3 matrix = v.Matrix;

                // Matrix
                w.Write(matrix.M11); w.Write(matrix.M12); w.Write(matrix.M13);
                w.Write(matrix.M21); w.Write(matrix.M22); w.Write(matrix.M23);
                w.Write(matrix.M31); w.Write(matrix.M32); w.Write(matrix.M33);

                // Head
                w.Write(v.Head.X);
                w.Write(v.Head.Y);
                w.Write(v.Head.Z);

                // Tail
                w.Write(v.Tail.X);
                w.Write(v.Tail.Y);
                w.Write(v.Tail.Z);
            }
        };

        public static readonly BinaryType<MeshCluster> MeshCluster =
        new()
        {
            Size = 64, //Matrix3x3 Matrix, Vector3 Center, Vector3 Extents, ushort PatchIndex, ushort TriangleCount);
            DisplayName = "MeshCluster",
            Read = r =>
            {
                return new MeshCluster(
                    new Matrix3x3(
                        r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                        r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                        r.ReadSingle(), r.ReadSingle(), r.ReadSingle()
                    ),
                    new Vector3(
                        r.ReadSingle(),
                        r.ReadSingle(),
                        r.ReadSingle()
                    ),
                    new Vector3(
                        r.ReadSingle(),
                        r.ReadSingle(),
                        r.ReadSingle()
                    ),
                    r.ReadUInt16(),
                    r.ReadUInt16()
                );
            },
            Write = (w, v) =>
            {
                Matrix3x3 matrix = v.Matrix;

                // Matrix
                w.Write(matrix.M11); w.Write(matrix.M12); w.Write(matrix.M13);
                w.Write(matrix.M21); w.Write(matrix.M22); w.Write(matrix.M23);
                w.Write(matrix.M31); w.Write(matrix.M32); w.Write(matrix.M33);

                // Head
                w.Write(v.Center.X);
                w.Write(v.Center.Y);
                w.Write(v.Center.Z);

                // Tail
                w.Write(v.Extents.X);
                w.Write(v.Extents.Y);
                w.Write(v.Extents.Z);

                // Patch Index and Triangle Count
                w.Write(v.PatchIndex);
                w.Write(v.TriangleCount);
            }
        };

        public static readonly BinaryType<ObjectBone> ObjectBone =
        new()
        {
            Size = 144, //The size of an ObjectBone is 32 bytes for the name, 23 floats for the transform, and 5 integers for the indices.
            DisplayName = "ObjectBone",
            Read = r =>
            {
                char[] name = r.ReadChars(32);

                // Remove trailing null characters while keeping the result as char[].
                int length = Array.FindLastIndex(name, c => c != '\0') + 1;
                char[] trimmedName = name[..length];

                return new ObjectBone(
                    trimmedName,
                    new Transform(
                        new Matrix4x4(
                            r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                            r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                            r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(),
                            r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()
                        ),
                        new Quaternion(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()),
                        new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle())
                    ),
                    r.ReadInt32(),
                    r.ReadInt32(),
                    r.ReadInt32(),
                    r.ReadInt32(),
                    r.ReadInt32()
                );
            },
            Write = (w, v) =>
            {
                Transform transform = v.Transform;
                Matrix4x4 matrix = transform.Matrix;
                Quaternion rotation = transform.Rotation;
                Vector3 translation = transform.Translation;

                // Always write exactly 32 characters.
                char[] name = new char[32];

                int length = Math.Min(v.Name.Length, 32);
                Array.Copy(v.Name, name, length);

                w.Write(name);

                w.Write(matrix.M11); w.Write(matrix.M12); w.Write(matrix.M13); w.Write(matrix.M14);
                w.Write(matrix.M21); w.Write(matrix.M22); w.Write(matrix.M23); w.Write(matrix.M24);
                w.Write(matrix.M31); w.Write(matrix.M32); w.Write(matrix.M33); w.Write(matrix.M34);
                w.Write(matrix.M41); w.Write(matrix.M42); w.Write(matrix.M43); w.Write(matrix.M44);
                w.Write(rotation.X); w.Write(rotation.Y); w.Write(rotation.Z); w.Write(rotation.W);
                w.Write(translation.X); w.Write(translation.Y); w.Write(translation.Z);

                w.Write(v.SkinID);
                w.Write(v.ParentIndex);
                w.Write(v.NextSiblingIndex);
                w.Write(v.FirstChildIndex);
                w.Write(v.Reserved);
            }
        };

        public static readonly BinaryType<VertexAttribute> VertexAttribute =
        new()
        {
            Size = 4, //Takes one and converts it to a VertexAttributeType
            DisplayName = "VertexAttribute",
            Read = r =>
            {
                uint descriptor = r.ReadUInt32();
                return new VertexAttribute(descriptor);
            },
            Write = (w, v) =>
            {
                w.Write((uint)v.RawDescriptor);
            }
        };
    }
}