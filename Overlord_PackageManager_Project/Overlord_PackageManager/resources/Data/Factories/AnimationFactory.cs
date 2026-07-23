using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class AnimationFactory
    {
        public static Entry CreateAnimationAssetDataContainer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                10 => new AssetListContainer(id, relOffset),     // List of all bone animations making up the entire animation, meaning each bone and its corresponding animation data
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),      // Unknown entry
            };
        }

        public static Entry CreateAnimationAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                01 => new AnimationAssetDataContainer(id, relOffset),   // Sub reference table containing a list of Bone Animation Data
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),                    // FFFF Block unkown use
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),                   // Chunk or In-Game Object Name
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),                   // Animation name
                30 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),                    // Unkown float and unknown use
                31 => new ScalarEntry<ulong>(id, relOffset, BinaryTypes.UInt64),                    // unkown u64 and unknown use
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}