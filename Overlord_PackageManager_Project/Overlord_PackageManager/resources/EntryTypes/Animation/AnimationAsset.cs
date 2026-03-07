using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class AnimationAsset(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelOffset;
            long end = start + Length;

            reader.BaseStream.Position = start;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is AnimationAssetDataContainer)
                {
                    ((AnimationAssetDataContainer)entry).Read(reader, Table.OffsetOrigin, AnimationAssetDataContainerDictionary);
                }
                else
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
