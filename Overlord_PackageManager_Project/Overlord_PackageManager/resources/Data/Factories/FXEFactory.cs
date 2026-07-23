using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    internal class FXEFactory
    {
        public static Entry CreateFXE(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                //20 => new ScalarEntry<int>(id, relOffset, BinaryTypes.Int32),    // Length of FXE Data
                //21 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),    // FXE Data, Header + Raw Data, full FXE file
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}