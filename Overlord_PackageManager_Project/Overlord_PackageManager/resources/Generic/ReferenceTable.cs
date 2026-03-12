using System.IO;

namespace Overlord_PackageManager.resources.Generic
{
    public class ReferenceTable
    {
        public bool HasLargeEntries;
        public uint SmallEntryCount;
        public uint LargeEntryCount = 0;
        public List<Entry> Entries;
        public long PayloadStartOffset;   // Start of entry payload region
        public long TableEndOffset;       // Absolute end boundary of this table

        public ReferenceTable()
        {

        }

        public void ReadHeader(BinaryReader reader)
        {
            byte temp = reader.ReadByte();
            HasLargeEntries = (temp & 0x80) != 0;
            SmallEntryCount = (uint)(temp & 0x7F);
            
            if (HasLargeEntries)
            {
                LargeEntryCount = reader.ReadUInt32();
            }

            PayloadStartOffset = reader.BaseStream.Position + (SmallEntryCount * 2L) + (LargeEntryCount * 8L);
        }

        public void ReadEntryStructure(BinaryReader reader, Func<BinaryReader, uint, uint, Entry> entryFactory)
        {
            int totalEntries = (int)(SmallEntryCount + LargeEntryCount);
            Entries = new List<Entry>(totalEntries);

            for(int i = 0; i < SmallEntryCount; i++)
            {
                byte id = reader.ReadByte();
                byte relOffset = reader.ReadByte();
                Entries.Add(entryFactory(reader, id, relOffset));
            }

            for (int i = 0; i < LargeEntryCount; i++)
            {
                uint id = reader.ReadUInt32();
                uint relOffset = reader.ReadUInt32();
                Entries.Add(entryFactory(reader, id, relOffset));
            }
            ComputeEntryLengths();
        }

        public void ReadAssetListEntryStructure(BinaryReader reader, Func<BinaryReader, uint, uint, long, Entry> entryFactory)
        {
            int totalEntries = (int)(SmallEntryCount + LargeEntryCount);
            Entries = new List<Entry>(totalEntries);

            for (int i = 0; i < SmallEntryCount; i++)
            {
                byte id = reader.ReadByte();
                byte relOffset = reader.ReadByte();
                Entries.Add(entryFactory(reader, id, relOffset, PayloadStartOffset));
            }

            for (int i = 0; i < LargeEntryCount; i++)
            {
                uint id = reader.ReadUInt32();
                uint relOffset = reader.ReadUInt32();
                Entries.Add(entryFactory(reader, id, relOffset, PayloadStartOffset));
            }
            ComputeEntryLengths();
        }

        private void ComputeEntryLengths()
        {
            Entries.Sort((a, b) => a.RelativeOffset.CompareTo(b.RelativeOffset));
            for (int i = 0; i < Entries.Count; i++)
            {
                long start = PayloadStartOffset + Entries[i].RelativeOffset;
                long end;

                if (i < Entries.Count - 1)
                    end = PayloadStartOffset + Entries[i + 1].RelativeOffset;
                else
                    end = TableEndOffset;

                Entries[i].PayloadLength = Math.Max(0, end - start);
            }
        }

        private void UpdateEntryStructure()
        {
            bool requiresLarge = false;
            uint smallCount = 0;
            uint largeCount = 0;

            foreach (var e in Entries)
            {
                if (e.Id > 0xFF || e.RelativeOffset > 0xFF)
                {
                    requiresLarge = true;
                    largeCount++;
                }
                else
                {
                    smallCount++;
                }
            }

            HasLargeEntries = requiresLarge;
            SmallEntryCount = smallCount;
            LargeEntryCount = largeCount;
        }

        private long GetTotalSize(long payloadSize)
        {
            long headerSize = 1 + (HasLargeEntries ? 4 : 0);

            long structureSize = SmallEntryCount * 2L + LargeEntryCount * 8L;

            PayloadStartOffset = headerSize + structureSize;

            return headerSize + structureSize + payloadSize;
        }

        public long ComputeLayout()
        {
            long currentOffset = 0;

            foreach (var entry in Entries)
            {
                long size = entry.GetPayloadSize();

                entry.PayloadLength = size;
                entry.RelativeOffset = (uint)currentOffset;

                currentOffset += size;
            }

            UpdateEntryStructure();

            return GetTotalSize(currentOffset);
        }

        public void Write(BinaryWriter writer, long tableStart)
        {
            writer.BaseStream.Position = tableStart;

            byte header = (byte)(SmallEntryCount & 0x7F);

            if (HasLargeEntries)
            {
                header |= 0x80;
            }

            writer.Write(header);

            if (HasLargeEntries)
            {
                writer.Write(LargeEntryCount);
            }

            foreach (var entry in Entries)
            {
                if (entry.Id <= 0xFF && entry.RelativeOffset <= 0xFF)
                {
                    writer.Write((byte)entry.Id);
                    writer.Write((byte)entry.RelativeOffset);
                }
                if (entry.Id > 0xFF || entry.RelativeOffset > 0xFF)
                {
                    writer.Write(entry.Id);
                    writer.Write(entry.RelativeOffset);
                }
            }

            long payloadOrigin = tableStart + PayloadStartOffset;

            foreach (var entry in Entries)
            {
                entry.Write(writer, payloadOrigin);
            }
        }
    }
}
