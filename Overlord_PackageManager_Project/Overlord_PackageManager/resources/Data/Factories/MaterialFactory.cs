using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Material;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class MaterialFactory
    {
        public static Entry CreateMaterialTextureLink(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // Texture Tag
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // Texture Name
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),      // Unknown entry
            };
        }
        public static Entry CreateMaskedPBRMaterial(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Unkown FFFFFF value
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char), // Material Tag
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char), // Material Name
                30 => new MaterialTextureLink(id, relOffset),   // Color Texture
                42 => new MaterialTextureLink(id, relOffset),   // Normal Texture
                43 => new MaterialTextureLink(id, relOffset),   // Reflection CubeMap Texture
                44 => new MaterialTextureLink(id, relOffset),   // Mask Texture
                45 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // Could be the color intensity or the opacity of the mask.
                46 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // Normal map strength or a smoothness/roughness value. Possibly threshold for masks (alpha clipping).
                47 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // Control specular power (shininess) or, if the material reflects metal, the intensity of the cubemap reflection.
                49 => new MaterialTextureLink(id, relOffset),   // Color Texture
                50 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // UV Scaling
                51 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // UV Scaling
                52 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // Unkown float

                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry
            };
        }

        public static Entry CreateBumpedDiffuseMaterial(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Unkown FFFFFF value
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char), // Material Tag
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char), // Material Name
                30 => new MaterialTextureLink(id, relOffset),   // Color Texture
                50 => new MaterialTextureLink(id, relOffset),   // Color Texture
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry
            };
        }

        public static Entry CreateDiffuseMaterial(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),            // Unkown FFFFFF value
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char), // Material Tag
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char), // Material Name
                30 => new MaterialTextureLink(id, relOffset),   // Color Texture
                41 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // Unkown float
                50 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),            // Unkown float
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry
            };
        }
    }
}
