using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                _ => new BlobEntry(id, relOffset),      // Unknown entry
            };
        }

        public static Entry CreateAnimationAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                01 => new AnimationAssetDataContainer(id, relOffset),   // Sub reference table containing a list of Bone Animation Data
                19 => new Int32Entry(id, relOffset),                    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),                   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),                   // Animation name
                30 => new FloatEntry(id, relOffset),                    // Unkown float and unknown use
                31 => new Int64Entry(id, relOffset),                    // unkown u64 and unknown use
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}