using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
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
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // Unkown u32
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationSubTableType23(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // Unkown u32
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateTranslationKeyframes(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                // Unkown u32
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                // Number of Bone positions, if the bone does not move in the animation only a single entry can be found here
                22 => new RawArrayEntry<BonePosition>(id, relOffset, BinaryTypes.BonePosition),// Array of Bone positions
                
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateRotationKeyframeData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                22 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                 // Number of Bone rotations
                23 => new RawArrayEntry<BoneRotation>(id, relOffset, BinaryTypes.BoneRotation), // Array of Bone rotations
                24 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),                 // Bitfield, potentially if its a keyframe or not, but this is not confirmed yet
                //30 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                 // Number of Bone scales
                //31 => new BoneScaleDataArray(id, relOffset),                                  // Number of Bone scales
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),                  // Unknown entry
            };
        }

        public static Entry CreateRotationKeyframes(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                                    // Unkown u32
                21 => new RotationKeyframeData(id, relOffset),     // Contains Bone Rotation and Scale data
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateBoneAnimationAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),     // Bone Name
                21 => new ScalarEntry<ulong>(id, relOffset, BinaryTypes.UInt64),        // Unkown u64
                22 => new BoneAnimationSubTableType22(id, relOffset),                   // Unkown use
                23 => new BoneAnimationSubTableType23(id, relOffset),                   // Unkown use
                24 => new TranslationKeyframes(id, relOffset),                          // Contains Keyframes with Bone Position
                25 => new RotationKeyframes(id, relOffset),                             // Contains Keyframes with Bone Rotation
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),          // Unknown entry
            };
        }
    }
}