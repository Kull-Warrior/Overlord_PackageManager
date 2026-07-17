using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class MeshFactory
    {
        public static Entry CreateVertexBufferInfo (BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new BlobEntry(id, relOffset),                 // Unkown single byte entry, maybe an ID of some sort
                21 => new UInt32Entry(id, relOffset),                // Single Data blob Stride
                22 => new UInt32Entry(id, relOffset),                // Uint number of attribute descriptors
                23 => new VertexDeclarationEntry(id, relOffset),    // Attribute descriptor table,  (uint)
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),                  // Unknown entry
            };
        }

        public static Entry CreateVertexBuffer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new VertexBufferInfo(id, relOffset),  // Contains metadata about the vertex buffer such as stride and attribute descriptors
                21 => new UInt32Entry(id, relOffset),            // Data Blob count
                22 => new BlobEntry(id, relOffset),             // Vertex count
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),              // Unknown entry
            };
        }

        public static Entry CreateIndiceData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new BlobEntry(id, relOffset),         // Unkown single byte entry
                21 => new UInt32Entry(id, relOffset),        // Indice count
                22 => new UInt16ArrayEntry(id, relOffset),  // Raw indice data
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),          // Unknown entry
            };
        }

        public static Entry CreateMeshBonebindPoseData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                21 => new UInt32Entry(id, relOffset),            // Item count
                22 => new MatricesArrayEntry(id, relOffset),    // Array of 4x4 bind pose matrices (16 floats each)
                _ => new BlobEntry(id, relOffset),              // Unknown entry 
            };
        }

        public static Entry CreateMeshBoneShapeData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                21 => new UInt32Entry(id, relOffset),                // Item count
                22 => new MeshBoneShapeArrayEntry(id, relOffset),   // Item count
                _ => new BlobEntry(id, relOffset),                  // Unkown entry
            };
        }

        public static Entry CreateMeshBoneData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new UInt32Entry(id, relOffset),            // Unkown uint
                21 => new UInt32Entry(id, relOffset),            // Bone data stride
                22 => new BlobEntry(id, relOffset),             // Raw Bone Data
                23 => new UInt32Entry(id, relOffset),            // Unkown uint
                24 => new MeshBoneBindPoseData(id, relOffset),  // Bind pose data for every bone, contains an array of 4x4 bind pose matrices (16 floats each)
                25 => new MeshBoneShapeData(id, relOffset),     // Bone shape data
                _ => new BlobEntry(id, relOffset),              // Unknown entry
            };
        }

        public static Entry CreateMeshClusterIndexBuffer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                22 => new UInt32Entry(id, relOffset),        // Cluster index count
                23 => new UInt16ArrayEntry(id, relOffset),  // Raw cluster index data
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),          // Unknown entry
            };
        }

        public static Entry CreateMeshClusterData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new UInt32Entry(id, relOffset),                // Count of Clusters
                21 => new RawMeshClusterDataArrayEntry(id, relOffset),   // Cluster data
                22 => new MeshClusterIndexBuffer(id, relOffset),    // Cluster index buffer
                _ => new BlobEntry(id, relOffset),                  // Unknown entry
            };
        }

        public static Entry CreateMeshData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                10 => new IndiceData(id, relOffset),        // Indice data
                11 => new VertexBuffer(id, relOffset),      // Vertice data
                13 => new MeshBoneData(id, relOffset),         // Unkown entry
                14 => new FloatArrayEntry(id, relOffset),   // Unkown float array entry likely a form of matrix data
                15 => new MeshClusterData(id, relOffset),   // Cluster data containing cluster index buffer and some unknown data blob
                16 => new FloatArrayEntry(id, relOffset),   // Unkown float array entry likely a form of matrix data
                17 => new BlobEntry(id, relOffset),         // Unkown entry
                18 => new BlobEntry(id, relOffset),         // Unkown entry
                19 => new BlobEntry(id, relOffset),         // Unkown entry
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),          // Unknown entry
            };
        }
        public static Entry CreateMeshAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new MeshData(id, relOffset),      // Sub reference table containing all mesh data
                19 => new UInt32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),    // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),    // Mesh Name
                50 => new BlobEntry(id, relOffset),     // Unknown entry
                51 => new BlobEntry(id, relOffset),     // Unknown entry
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}