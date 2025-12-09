using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneAnimationData(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public uint entryTypeIdentifier;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            entryTypeIdentifier = reader.ReadUInt32();
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                if (entry is Int64Entry || entry is StringEntry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is BoneAnimationSubTableType22)
                {
                    ((BoneAnimationSubTableType22)entry).Read(reader, varRefTable.origin, BoneAnimationSubTableType22Dictionary);
                }
                if (entry is BoneAnimationSubTableType23)
                {
                    ((BoneAnimationSubTableType23)entry).Read(reader, varRefTable.origin, BoneAnimationSubTableType23Dictionary);
                }
                if (entry is BoneAnimationSubTableType24)
                {
                    ((BoneAnimationSubTableType24)entry).Read(reader, varRefTable.origin, BoneAnimationSubTableType24Dictionary);
                }
                if (entry is BoneAnimationSubTableType25)
                {
                    ((BoneAnimationSubTableType25)entry).Read(reader, varRefTable.origin, BoneAnimationSubTableType25Dictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
