using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                20 => new StringEntry(id, relOffset),   // Texture Tag
                21 => new StringEntry(id, relOffset),   // Texture Name
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),      // Unknown entry
            };
        }
        public static Entry CreateMaskedPBRMaterial(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                19 => new Int32Entry(id, relOffset),            // Unkown FFFFFF value
                20 => new StringEntry(id, relOffset),           // Material Tag
                21 => new StringEntry(id, relOffset),           // Material Name
                30 => new MaterialTextureLink(id, relOffset),   // Color Texture
                42 => new MaterialTextureLink(id, relOffset),   // Normal Texture
                43 => new MaterialTextureLink(id, relOffset),   // Reflection CubeMap Texture
                44 => new MaterialTextureLink(id, relOffset),   // Mask Texture
                45 => new FloatEntry(id, relOffset),            // Could be the color intensity or the opacity of the mask.
                46 => new FloatEntry(id, relOffset),            // Normal map strength or a smoothness/roughness value. Possibly threshold for masks (alpha clipping).
                47 => new FloatEntry(id, relOffset),            // Control specular power (shininess) or, if the material reflects metal, the intensity of the cubemap reflection.
                49 => new MaterialTextureLink(id, relOffset),   // Color Texture
                50 => new FloatEntry(id, relOffset),            // UV Scaling
                51 => new FloatEntry(id, relOffset),            // UV Scaling
                52 => new FloatEntry(id, relOffset),            // Unkown float

                _ => new BlobEntry(id, relOffset),              // Unknown entry
            };
        }

        public static Entry CreateBumpedDiffuseMaterial(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                19 => new Int32Entry(id, relOffset),            // Unkown FFFFFF value
                20 => new StringEntry(id, relOffset),           // Material Tag
                21 => new StringEntry(id, relOffset),           // Material Name
                30 => new MaterialTextureLink(id, relOffset),   // Color Texture
                50 => new MaterialTextureLink(id, relOffset),   // Color Texture
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),              // Unknown entry
            };
        }

        public static Entry CreateDiffuseMaterial(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                19 => new Int32Entry(id, relOffset),            // Unkown FFFFFF value
                20 => new StringEntry(id, relOffset),           // Material Tag
                21 => new StringEntry(id, relOffset),           // Material Name
                30 => new MaterialTextureLink(id, relOffset),   // Color Texture
                41 => new FloatEntry(id, relOffset),            // Unkown float
                50 => new FloatEntry(id, relOffset),            // Unkown float
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),              // Unknown entry
            };
        }
    }
}
