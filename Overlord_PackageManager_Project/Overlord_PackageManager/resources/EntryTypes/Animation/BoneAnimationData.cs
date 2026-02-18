using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneAnimationData(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is Int64Entry || entry is StringEntry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is BoneAnimationSubTableType22)
                {
                    ((BoneAnimationSubTableType22)entry).Read(reader, Table.OffsetOrigin, BoneAnimationSubTableType22Dictionary);
                }
                if (entry is BoneAnimationSubTableType23)
                {
                    ((BoneAnimationSubTableType23)entry).Read(reader, Table.OffsetOrigin, BoneAnimationSubTableType23Dictionary);
                }
                if (entry is BoneAnimationSubTableType24)
                {
                    ((BoneAnimationSubTableType24)entry).Read(reader, Table.OffsetOrigin, BoneAnimationSubTableType24Dictionary);
                }
                if (entry is BoneAnimationSubTableType25)
                {
                    ((BoneAnimationSubTableType25)entry).Read(reader, Table.OffsetOrigin, BoneAnimationSubTableType25Dictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
