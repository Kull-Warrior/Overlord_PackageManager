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
        public ReferenceTable(BinaryReader reader, long tableEnd, Func<uint, uint, Entry> entryFactory)
        {
            TableEndOffset = tableEnd;
            byte temp = reader.ReadByte();
            HasLargeEntries = Convert.ToBoolean(temp >> 7);
            SmallEntryCount = (uint)(temp & 0x7F);

            if (HasLargeEntries == true)
            {
                LargeEntryCount = reader.ReadUInt32();
            }

            for (int i = 0; i < SmallEntryCount; i++)
            {
                uint id = reader.ReadByte();
                uint offset = reader.ReadByte();

                Entries.Add(entryFactory(id,offset));
            }

            for (int i = 0; i < LargeEntryCount; i++)
            {
                uint id = reader.ReadUInt32();
                uint offset = reader.ReadUInt32();

                Entries.Add(entryFactory(id, offset));
            }

            PayloadStartOffset = reader.BaseStream.Position;
            ComputeEntryLengths();
        }

        public ReferenceTable(BinaryReader reader, long tableEnd)
        {
            TableEndOffset = tableEnd;
            byte temp = reader.ReadByte();
            HasLargeEntries = Convert.ToBoolean(temp >> 7);
            SmallEntryCount = (uint)(temp & 0x7F);

            List<uint> ids = new List<uint>();
            List<uint> relativeOffsets = new List<uint>();

            if (HasLargeEntries == true)
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
                reader.BaseStream.Position = PayloadStartOffset + relativeOffsets[i];

                uint typeIdentifier = reader.ReadUInt32();

                switch (typeIdentifier)
                {
                    /*case 4259915:   // Object -> Meshes & used materials by these meshs assignment block.  Skeleton Data as well
                        break;
                    case 4261412:   // Material -> Used textures assignment block. So Skin, ReflectionCubeMap, etc etc
                        break;*/
                    case 4259993:
                        Entries.Add(new ReflectionCubeMapAsset(ids[i], relativeOffsets[i]));
                        break;
                    case 4259992:   // Tif Image, tga32 Image
                        Entries.Add(new TgaTifTextureAsset(ids[i], relativeOffsets[i]));
                        break;
                    case 4259901:   // DDS Texture Asset
                        Entries.Add(new DDSTextureAsset(ids[i], relativeOffsets[i]));
                        break;
                    case 4259876:   // Raw DDS Texture Data
                        Entries.Add(new DDSTextures(ids[i], relativeOffsets[i]));
                        break;
                    case 4259845:   // Animation Asset
                        Entries.Add(new AnimationAsset(ids[i], relativeOffsets[i]));
                        break;
                    case 4259847:   // Bone Animation Data
                        Entries.Add(new BoneAnimationData(ids[i], relativeOffsets[i]));
                        break;
                    /*case 4259893:   // Mesh Asset
                        break;*/
                    case 10551296:  // SFX Asset
                        Entries.Add(new SFXAsset(ids[i], relativeOffsets[i]));
                        break;
                    default:
                        Entries.Add(new BlobEntry(ids[i], relativeOffsets[i]));
                        break;
                }
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
