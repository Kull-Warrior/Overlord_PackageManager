using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.IO;

namespace Overlord_PackageManager.resources.Generic
{
    public class RefTable
    {
        public bool HasBigEntry;
        public int Count8;
        public int Count32 = 0;
        public List<Entry> Entries = new List<Entry>();
        public long origin;

        public RefTable(BinaryReader reader, Func<uint, uint, Entry> entryFactory)
        {
            byte temp = reader.ReadByte();
            HasBigEntry = Convert.ToBoolean(temp >> 7);
            Count8 = temp & 0x7F;

            if (HasBigEntry == true)
            {
                Count32 = reader.ReadInt32();
            }

            for (int i = 0; i < Count8; i++)
            {
                uint id = reader.ReadByte();
                uint offset = reader.ReadByte();

                Entries.Add(entryFactory(id,offset));
            }

            for (int i = 0; i < Count32; i++)
            {
                uint id = (uint)reader.ReadInt32();
                uint offset = (uint)reader.ReadInt32();

                Entries.Add(entryFactory(id, offset));
            }

            origin = reader.BaseStream.Position;
        }

        public RefTable(BinaryReader reader)
        {
            byte temp = reader.ReadByte();
            HasBigEntry = Convert.ToBoolean(temp >> 7);
            Count8 = temp & 0x7F;

            List<uint> ids = new List<uint>();
            List<uint> relativeOffsets = new List<uint>();

            if (HasBigEntry == true)
            {
                Count32 = reader.ReadInt32();
            }

            for (int i = 0; i < Count8; i++)
            {
                ids.Add(reader.ReadByte());
                relativeOffsets.Add(reader.ReadByte());
            }

            for (int i = 0; i < Count32; i++)
            {
                ids.Add(reader.ReadUInt32());
                relativeOffsets.Add(reader.ReadUInt32());
            }

            origin = reader.BaseStream.Position;

            for (int i = 0; i < ids.Count; i++)
            {
                reader.BaseStream.Position = origin + relativeOffsets[i];

                uint identifier = reader.ReadUInt32();

                switch (identifier)
                {
                    case 4259915:   // Object -> Meshes & used materials by these meshs assignment block.  Skeleton Data as well
                        break;
                    case 4261412:   // Material -> Used textures assignment block. So Skin, reflectionmap, etc etc
                        break;
                    case 4259992:   // Tif Image, tga32 Image
                        Entries.Add(new TgaTifTextureAsset(ids[i], relativeOffsets[i]));
                        break;
                    case 4259901:   // DDS Texture Asset
                        Entries.Add(new DDSTextureAsset(ids[i], relativeOffsets[i]));
                        break;
                    case 4259876:   // Raw DDS Texture Data
                        Entries.Add(new RawDDSTextureData(ids[i], relativeOffsets[i]));
                        break;
                    case 4259845:   // Animation Asset
                        break;
                    case 4259893:   // Mesh Asset
                        break;
                    case 10551296:  // SFX Asset
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
