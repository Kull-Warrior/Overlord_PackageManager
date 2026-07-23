using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
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
                30 => new UInt32Entry(id, relOffset),    // Length of SFX Data
                31 => new ByteArrayEntry(id, relOffset),    // SFX Data, Header + Raw Audio Data, full wav style file
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateSFXAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new SFXData(id, relOffset),        // Sub reference table containing a int32 and a full wav style file
                19 => new UInt32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new CharCountedArrayEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new CharCountedArrayEntry(id, relOffset),   // Sound name
                100 => new CharCountedArrayEntry(id, relOffset),  // File name
                101 => new UInt32Entry(id, relOffset),   // FFFF Block unkown use
                104 => new UInt32Entry(id, relOffset),    // Unkown int32
                105 => new ByteEntry(id, relOffset),    // Unkown single byte
                106 => new UInt32Entry(id, relOffset),   // Unkown int32
                107 => new UInt32Entry(id, relOffset),   // Unkown int32
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}