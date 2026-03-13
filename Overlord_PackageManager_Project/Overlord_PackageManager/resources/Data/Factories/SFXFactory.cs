using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class SFXFactory
    {
        public static Entry CreateSFXData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),    // Length of SFX Data
                31 => new BlobEntry(id, relOffset),    // SFX Data, Header + Raw Audio Data, full wav style file
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateSFXAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new SFXData(id, relOffset),        // Sub reference table containing a int32 and a full wav style file
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // Sound name
                100 => new StringEntry(id, relOffset),  // File name
                101 => new Int32Entry(id, relOffset),   // FFFF Block unkown use
                104 => new Int32Entry(id, relOffset),    // Unkown int32
                105 => new SingleByteEntry(id, relOffset),    // Unkown single byte
                106 => new Int32Entry(id, relOffset),   // Unkown int32
                107 => new Int32Entry(id, relOffset),   // Unkown int32
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}