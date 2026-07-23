using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth;
using Overlord_PackageManager.resources.Data.EntryTypes.Lua;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class LuaFactory
    {
        public static Entry CreateLuaData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                21 => new CharListCountedArrayEntry(id, relOffset),
                22 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                23 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateLuaList(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new LuaEntry(id, relOffset) //Any ID is of this type
            };
        }

        public static Entry CreateLuaAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                10 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                11 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                30 => new LuaEntry(id, relOffset),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}