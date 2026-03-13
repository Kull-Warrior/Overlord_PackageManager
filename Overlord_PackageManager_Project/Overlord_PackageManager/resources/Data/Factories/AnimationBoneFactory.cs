using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class AnimationBoneFactory
    {
        public static Entry CreateBoneAnimationSubTableType22(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Unkown u32
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationSubTableType23(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Unkown u32
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationSubTableType24(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),                // Unkown u32
                21 => new Int32Entry(id, relOffset),                // Number of Bone positions, if the bone does not move in the animation only a single entry can be found here
                //22 => new BonePositionDataArray(id, relOffset),     // Array of Bone positions
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationSubTableType25SubTableType21(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                22 => new Int32Entry(id, relOffset),                // Number of Bone rotations
                //23 => new BoneRotationDataArray(id, relOffset),     // Array of Bone rotations
                24 => new BlobEntry(id, relOffset),               // Unkown 12 Bytes
                30 => new Int32Entry(id, relOffset),                // Number of Bone scales
                //31 => new BoneScaleDataArray(id, relOffset),        // Number of Bone scales
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationSubTableType25(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),                                    // Unkown u32
                21 => new BoneAnimationSubTableType25SubTableType21(id, relOffset),     // Contains Bone Rotation and Scale data
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new StringEntry(id, relOffset),                   // Bone Name
                21 => new Int64Entry(id, relOffset),                    // Unkown u64
                22 => new BoneAnimationSubTableType22(id, relOffset),   // Unkown use
                23 => new BoneAnimationSubTableType23(id, relOffset),   // Unkown use
                24 => new BoneAnimationSubTableType24(id, relOffset),   // Contains Bone Position data at deeper levels
                25 => new BoneAnimationSubTableType25(id, relOffset),   // Contains Bone Rotation and Scale data at deeper levels
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}