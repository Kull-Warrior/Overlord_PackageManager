using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Object;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class ObjectFactory
    {
        public static Entry CreateObjectBoneContainer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                22 => new RawArrayEntry<ObjectBone>(id, relOffset, BinaryTypes.ObjectBone),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateObjectAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                01 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                30 => new TableEntry(id, relOffset),
                32 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),
                33 => new ObjectBoneContainer(id, relOffset),
                34 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),
                35 => new TableEntry(id, relOffset),
                36 => new TableEntry(id, relOffset),

                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}