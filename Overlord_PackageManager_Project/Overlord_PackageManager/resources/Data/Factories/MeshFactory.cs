using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class MeshFactory
    {
        public static Entry CreateVertexBufferInfo (BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),                 // Unkown single byte entry, maybe an ID of some sort
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                // Single Data blob Stride
                22 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                // Uint number of attribute descriptors
                23 => new RawListEntry<VertexAttribute>(id, relOffset, BinaryTypes.VertexAttribute),    // Attribute descriptor table,  (uint)
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),                  // Unknown entry
            };
        }

        public static Entry CreateVertexBuffer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new VertexBufferInfo(id, relOffset),  // Contains metadata about the vertex buffer such as stride and attribute descriptors
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Data Blob count
                22 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),             // Vertex count
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry
            };
        }

        public static Entry CreateIndiceData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),         // Unkown single byte entry
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),        // Indice count
                22 => new RawArrayEntry<ushort>(id, relOffset, BinaryTypes.UInt16),  // Raw indice data
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),          // Unknown entry
            };
        }

        public static Entry CreateMeshBonebindPoseData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Item count
                22 => new RawArrayEntry<Matrix4x4>(id, relOffset, BinaryTypes.Matrix4x4),    // Array of 4x4 bind pose matrices (16 floats each)
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry 
            };
        }

        public static Entry CreateMeshBoneShapeData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                // Item count
                22 => new RawArrayEntry<MeshBoneShape>(id, relOffset, BinaryTypes.MeshBoneShape),   // Item count
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),                  // Unkown entry
            };
        }

        public static Entry CreateMeshBoneData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Unkown uint
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Bone data stride
                22 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),             // Raw Bone Data
                23 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Unkown uint
                24 => new MeshBoneBindPoseData(id, relOffset),  // Bind pose data for every bone, contains an array of 4x4 bind pose matrices (16 floats each)
                25 => new MeshBoneShapeData(id, relOffset),     // Bone shape data
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry
            };
        }

        public static Entry CreateMeshClusterIndexBuffer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                22 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),        // Cluster index count
                23 => new RawArrayEntry<ushort>(id, relOffset, BinaryTypes.UInt16),  // Raw cluster index data
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),          // Unknown entry
            };
        }

        public static Entry CreateMeshClusterData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                // Count of Clusters
                21 => new RawArrayEntry<RawMeshClusterData>(id, relOffset, BinaryTypes.RawMeshClusterData),   // Cluster data
                22 => new MeshClusterIndexBuffer(id, relOffset),    // Cluster index buffer
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),                  // Unknown entry
            };
        }

        public static Entry CreateMeshData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                10 => new IndiceData(id, relOffset),        // Indice data
                11 => new VertexBuffer(id, relOffset),      // Vertice data
                13 => new MeshBoneData(id, relOffset),         // Unkown entry
                14 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),   // Unkown float array entry likely a form of matrix data
                15 => new MeshClusterData(id, relOffset),   // Cluster data containing cluster index buffer and some unknown data blob
                16 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),   // Unkown float array entry likely a form of matrix data
                17 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),         // Unkown entry
                18 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),         // Unkown entry
                19 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),         // Unkown entry
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),          // Unknown entry
            };
        }
        public static Entry CreateMeshAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new MeshData(id, relOffset),      // Sub reference table containing all mesh data
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // FFFF Block unkown use
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),    // Chunk or In-Game Object Name
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),    // Mesh Name
                50 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),     // Unknown entry
                51 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),     // Unknown entry
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}