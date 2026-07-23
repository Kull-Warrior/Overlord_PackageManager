using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class XMLFactory
    {
        public static Entry CreateXML(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                10 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                11 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                12 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}