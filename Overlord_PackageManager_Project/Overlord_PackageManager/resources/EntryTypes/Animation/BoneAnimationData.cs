using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneAnimationData(uint id, uint relOffset) : AssetEntry(id, relOffset)
    {
        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is BoneAnimationSubTableType22)
                {
                    ((BoneAnimationSubTableType22)entry).Read(reader, Table.PayloadStartOffset, BoneAnimationSubTableType22Dictionary);
                }
                else if (entry is BoneAnimationSubTableType23)
                {
                    ((BoneAnimationSubTableType23)entry).Read(reader, Table.PayloadStartOffset, BoneAnimationSubTableType23Dictionary);
                }
                else if (entry is BoneAnimationSubTableType24)
                {
                    ((BoneAnimationSubTableType24)entry).Read(reader, Table.PayloadStartOffset, BoneAnimationSubTableType24Dictionary);
                }
                else if (entry is BoneAnimationSubTableType25)
                {
                    ((BoneAnimationSubTableType25)entry).Read(reader, Table.PayloadStartOffset, BoneAnimationSubTableType25Dictionary);
                }
                else
                {
                    entry.Read(reader, Table.PayloadStartOffset);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
