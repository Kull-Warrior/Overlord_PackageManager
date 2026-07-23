using Overlord_PackageManager.resources.Data.DataTypes;
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
                30 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // Length of SFX Data
                31 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),    // SFX Data, Header + Raw Audio Data, full wav style file
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateSFXAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new SFXData(id, relOffset),        // Sub reference table containing a int32 and a full wav style file
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // FFFF Block unkown use
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // Chunk or In-Game Object Name
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // Sound name
                100 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),  // File name
                101 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),   // FFFF Block unkown use
                104 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // Unkown int32
                105 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),    // Unkown single byte
                106 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),   // Unkown int32
                107 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),   // Unkown int32
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}