using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class AnimationAsset(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] identifier;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            identifier = reader.ReadBytes(4);
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                if(entry is StringEntry || entry is Int32Entry || entry is Int64Entry || entry is FloatEntry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is AnimationAssetSubTableType1)
                {
                    ((AnimationAssetSubTableType1)entry).Read(reader, varRefTable.origin, AnimationAssetSubTableType1Dictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
