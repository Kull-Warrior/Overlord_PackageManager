using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class DDSTextureFactory
    {
        public static Entry CreateDDSTexture(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Image width
                21 => new Int32Entry(id, relOffset),    // Image height
                22 => new BlobEntry(id, relOffset),   // Raw image data
                23 => new Int32Entry(id, relOffset),    // DDS Format
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateDDSTextureAssetDataContainer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new AssetListContainer(id, relOffset),     // List of all dds images making up the entire dds file, meaning the main image and each mipmap
                21 => new Int32Entry(id, relOffset),     // Unkown
                23 => new Int32Entry(id, relOffset),     // Unkown
                24 => new BlobEntry(id, relOffset),   // Unknown entry
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateDDSTextureAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetDataContainer(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // File name
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateReflectionCubeMapAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetDataContainer(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // File name
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}