using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class AnimationAsset(uint id, uint relOffset) : AssetEntry(id, relOffset)
    {
        public void Read(BinaryReader reader, long origin, Func<BinaryReader, uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is AnimationAssetDataContainer)
                {
                    ((AnimationAssetDataContainer)entry).Read(reader, Table.PayloadStartOffset, AnimationAssetDataContainerDictionary);
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
