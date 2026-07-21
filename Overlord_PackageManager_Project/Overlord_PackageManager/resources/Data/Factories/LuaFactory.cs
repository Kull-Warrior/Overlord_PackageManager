using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
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
                20 => new StringEntry(id, relOffset),
                21 => new StringCountedListEntry(id, relOffset),
                22 => new UInt32Entry(id, relOffset),
                23 => new ByteArrayEntry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
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
                10 => new UInt32Entry(id, relOffset),
                11 => new UInt32Entry(id, relOffset),
                30 => new LuaEntry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}