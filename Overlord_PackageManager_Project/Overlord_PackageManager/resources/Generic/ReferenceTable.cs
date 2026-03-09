using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.IO;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.Animation;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;

namespace Overlord_PackageManager.resources.Generic
{
    public class ReferenceTable
    {
        public bool HasLargeEntries;
        public uint SmallEntryCount;
        public uint LargeEntryCount = 0;
        public List<Entry> Entries = new List<Entry>();
        public long PayloadStartOffset;   // Start of entry payload region
        public long TableEndOffset;       // Absolute end boundary of this table

        public ReferenceTable()
        {

        }
        public ReferenceTable(BinaryReader reader, long tableEnd, Func<BinaryReader, uint, uint, Entry> entryFactory)
        {
            TableEndOffset = tableEnd;
            byte temp = reader.ReadByte();
            HasLargeEntries = Convert.ToBoolean(temp >> 7);
            SmallEntryCount = (uint)(temp & 0x7F);

            List<uint> ids = new();
            List<uint> relativeOffsets = new();

            if (HasLargeEntries)
            {
                LargeEntryCount = reader.ReadUInt32();
            }

            for (int i = 0; i < SmallEntryCount; i++)
            {
                ids.Add(reader.ReadByte());
                relativeOffsets.Add(reader.ReadByte());
            }

            for (int i = 0; i < LargeEntryCount; i++)
            {
                ids.Add(reader.ReadUInt32());
                relativeOffsets.Add(reader.ReadUInt32());
            }

            PayloadStartOffset = reader.BaseStream.Position;

            for (int i = 0; i < ids.Count; i++)
            {
                Entries.Add(entryFactory(reader, ids[i], relativeOffsets[i]));
            }

            ComputeEntryLengths();
        }

        private void ComputeEntryLengths()
        {
            Entries = Entries.OrderBy(e => e.RelativeOffset).ToList();
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
    }
}
