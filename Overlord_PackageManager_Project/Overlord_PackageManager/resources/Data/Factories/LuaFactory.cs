using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                21 => new StringListEntry(id, relOffset),
                22 => new Int32Entry(id, relOffset),
                23 => new BlobEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
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
    }
}