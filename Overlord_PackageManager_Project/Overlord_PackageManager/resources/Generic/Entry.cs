using Overlord_PackageManager.resources.EntryTypes;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.EntryTypes.Lua;
using Overlord_PackageManager.resources.EntryTypes.XML;
using Overlord_PackageManager.resources.Generic.EntryTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Generic
{
    public abstract class Entry
    {
        public uint Id;
        public uint RelOffset;

        protected Entry()
        {

        }

        protected Entry(uint id, uint relOffset)
        {
            Id = id;
            RelOffset = relOffset;
        }

        // Each entry knows how to read itself
        public abstract void Read(BinaryReader reader, long origin);

        public static Entry InfoTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),
                21 => new Int32Entry(id, relOffset),
                22 => new RefTableEntry(id, relOffset),
                30 => new StringEntry(id, relOffset),
                31 => new Int32Entry(id, relOffset),
                32 => new BinaryEntry(id, relOffset),
                33 => new BinaryEntry(id, relOffset),
                36 => new Int32Entry(id, relOffset),
                37 => new Int32Entry(id, relOffset),
                38 => new Int32Entry(id, relOffset),
                39 => new Int32Entry(id, relOffset),
                40 => new StringEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry OMPDataRootTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new TerrainDataEntry(id, relOffset),
                21 => new UnknownTableType21Entry(id, relOffset),
                22 => new BinaryEntry(id, relOffset),
                23 => new BinaryEntry(id, relOffset),
                24 => new BinaryEntry(id, relOffset),
                25 => new LuaListEntry(id, relOffset),
                26 => new RefTableEntry(id, relOffset),
                27 => new RefTableEntry(id, relOffset),
                28 => new RefTableEntry(id, relOffset),
                29 => new ByteEntry(id, relOffset),
                30 => new RefTableEntry(id, relOffset),
                31 => new BinaryEntry(id, relOffset),
                32 => new RPKListEntry(id, relOffset),
                33 => new LuaEntry(id, relOffset),
                34 => new StringEntry(id, relOffset),
                35 => new RefTableEntry(id, relOffset),
                36 => new LuaEntry(id, relOffset),
                37 => new FloatEntry(id, relOffset),
                38 => new FloatEntry(id, relOffset),
                39 => new FloatEntry(id, relOffset),
                40 => new ByteEntry(id, relOffset),
                41 => new ByteEntry(id, relOffset),
                42 => new Int32Entry(id, relOffset),
                43 => new Int32Entry(id, relOffset),
                45 => new LuaEntry(id, relOffset),
                46 => new StringArrayEntry(id, relOffset),
                47 => new RefTableEntry(id, relOffset),
                48 => new RefTableEntry(id, relOffset),
                49 => new Int32Entry(id, relOffset),
                51 => new Int32Entry(id, relOffset),
                52 => new RefTableEntry(id, relOffset),
                53 => new RefTableEntry(id, relOffset),
                100 => new RefTableEntry(id, relOffset),
                101 => new RefTableEntry(id, relOffset),
                102 => new RefTableEntry(id, relOffset),
                103 => new RefTableEntry(id, relOffset),
                104 => new Int32Entry(id, relOffset),
                106 => new ByteEntry(id, relOffset),
                108 => new ByteEntry(id, relOffset),
                110 => new Int32Entry(id, relOffset),
                111 => new Int32Entry(id, relOffset),
                112 => new Int32Entry(id, relOffset),
                113 => new Int32Entry(id, relOffset),
                114 => new StringEntry(id, relOffset),
                115 => new Int32Entry(id, relOffset),
                116 => new ByteEntry(id, relOffset),
                117 => new ByteEntry(id, relOffset),
                120 => new RefTableEntry(id, relOffset),
                121 => new Int32Entry(id, relOffset),
                122 => new LuaEntry(id, relOffset),
                123 => new LuaEntry(id, relOffset),
                124 => new RefTableEntry(id, relOffset),
                125 => new Int32Entry(id, relOffset),
                126 => new Int32Entry(id, relOffset),
                127 => new LuaEntry(id, relOffset),
                128 => new Int32Entry(id, relOffset),
                129 => new Int32Entry(id, relOffset),
                130 => new RefTableEntry(id, relOffset),
                131 => new RefTableEntry(id, relOffset),
                132 => new BinaryEntry(id, relOffset),
                133 => new RefTableEntry(id, relOffset),
                134 => new StringEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry TerrainDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),
                31 => new Int32Entry(id, relOffset),
                33 => new BinaryEntry(id, relOffset),
                34 => new StringEntry(id, relOffset),
                35 => new StringEntry(id, relOffset),
                36 => new FloatEntry(id, relOffset),
                37 => new BinaryEntry(id, relOffset),
                38 => new ByteEntry(id, relOffset),
                39 => new BinaryEntry(id, relOffset),
                40 => new ByteEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry UnknownType21Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new RefTableEntry(id, relOffset),
                21 => new Int32Entry(id, relOffset),
                22 => new ByteEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry UnknownType21SubTableType20Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),
                31 => new StringEntry(id, relOffset),
                32 => new RefTableEntry(id, relOffset),
                34 => new RefTableEntry(id, relOffset),
                35 => new Int32Entry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry GameObjectDataSubTableType20SubTableType32Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                10 => new Int32Entry(id, relOffset),
                11 => new Int32Entry(id, relOffset),
                12 => new StringEntry(id, relOffset),
                20 => new BinaryEntry(id, relOffset),
                21 => new BinaryEntry(id, relOffset),
                22 => new Int32Entry(id, relOffset),
                23 => new Int32Entry(id, relOffset),
                24 => new Int32Entry(id, relOffset),
                40 => new ByteEntry(id, relOffset),
                42 => new ByteEntry(id, relOffset),
                60 => new StringEntry(id, relOffset),
                61 => new RefTableEntry(id, relOffset),
                62 => new RefTableEntry(id, relOffset),
                63 => new ByteEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry DataSubTableType21SubTableType20SubTableType34Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                22 => new Int32Entry(id, relOffset),
                23 => new Int32ArrayEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry LuaDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new StringEntry(id, relOffset),
                21 => new StringArrayEntry(id, relOffset),
                22 => new Int32Entry(id, relOffset),
                23 => new BinaryEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry LuaListDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new LuaEntry(id, relOffset) //Any ID is of this type
            };
        }

        public static Entry XMLDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                10 => new StringEntry(id, relOffset),
                11 => new Int32Entry(id, relOffset),
                12 => new BinaryEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry RPKListDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new RPKEntry(id, relOffset) //Any ID is of this type
            };
        }

        public static Entry RPKDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),
                31 => new StringEntry(id, relOffset),
                32 => new StringEntry(id, relOffset),
                33 => new ByteEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry RPKRootDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                19 => new Int32Entry(id, relOffset),
                20 => new Int32Entry(id, relOffset),
                22 => new StringEntry(id, relOffset),       // Character/Rescource Name
                23 => new RefTableEntry(id, relOffset),     // Contains Effects and Light Data of some sort
                24 => new RefTableEntry(id, relOffset),     // Contains "TS", Texture related Data
                25 => new RefTableEntry(id, relOffset),     // Contains "0" Type Data Unknown use, Map editor friendly apperently
                26 => new AssetList(id, relOffset),         // Contains Object, Mesh, Material, Texture, SFX, Animation and Shader Data
                27 => new StringArrayEntry(id, relOffset),  // Contains a list of strings. Reference to one or more .CLB files.
                28 => new XMLEntry(id, relOffset),          // Contains XML Data
                30 => new RefTableEntry(id, relOffset),     // Contains "2" Type Data Unknown use, contains lua scripts
                31 => new RefTableEntry(id, relOffset),     // Contains Animation Data
                32 => new RefTableEntry(id, relOffset),     // Unknown
                33 => new RefTableEntry(id, relOffset),     // Contains Object, Mesh Data //MapEditor Objects apperently
                34 => new RefTableEntry(id, relOffset),     // Contains Object Data, Animation Data
                35 => new RefTableEntry(id, relOffset),     // Unknown
                36 => new RefTableEntry(id, relOffset),     // Unknown
                37 => new RefTableEntry(id, relOffset),     // Contains SFX Data
                38 => new RefTableEntry(id, relOffset),     // Contains FXE Data ( Raw file)
                39 => new RefTableEntry(id, relOffset),     // 
                40 => new RefTableEntry(id, relOffset),
                60 => new RefTableEntry(id, relOffset),
                61 => new RefTableEntry(id, relOffset),
                62 => new RefTableEntry(id, relOffset),
                63 => new RefTableEntry(id, relOffset),
                64 => new RefTableEntry(id, relOffset),
                65 => new RefTableEntry(id, relOffset),
                66 => new RefTableEntry(id, relOffset),
                67 => new RefTableEntry(id, relOffset),
                68 => new RefTableEntry(id, relOffset),
                69 => new RefTableEntry(id, relOffset),
                70 => new RefTableEntry(id, relOffset),
                71 => new RefTableEntry(id, relOffset),
                72 => new RefTableEntry(id, relOffset),
                73 => new RefTableEntry(id, relOffset),
                74 => new RefTableEntry(id, relOffset),
                75 => new RefTableEntry(id, relOffset),
                77 => new RefTableEntry(id, relOffset),
                78 => new RefTableEntry(id, relOffset),
                80 => new RefTableEntry(id, relOffset),
                81 => new RefTableEntry(id, relOffset),
                82 => new XMLEntry(id, relOffset),          // Contains XML Data (System_Quests)
                83 => new XMLEntry(id, relOffset),          // Contains XML Data (Tower_GeneralMessages)
                84 => new XMLEntry(id, relOffset),          // Contains XML Data (TutorialMessages_Mouse)
                85 => new XMLEntry(id, relOffset),          // Contains XML Data (TutorialMessages_Gamepad)
                86 => new RefTableEntry(id, relOffset),     // Contains CPT_ Data
                87 => new RefTableEntry(id, relOffset),     // Contains CPTX Data
                88 => new RefTableEntry(id, relOffset),     // "Contains a string. with : 0D = Carriage Return(CR) 0A = Line Feed(LF) to break lines. Referencing here the multiplayer maps"
                89 => new RefTableEntry(id, relOffset),
                90 => new XMLEntry(id, relOffset),          // Contains XML Data (Credits)
                91 => new RefTableEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry RPKDataRootGenericSubTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new StringEntry(id, relOffset),
                21 => new RefTableEntry(id, relOffset),
                30 => new RefTableEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry AnimationEntryDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                //01  => new BoneAnimationEntry(id, relOffset),
                19 => new Int32Entry(id, relOffset),
                20 => new StringEntry(id, relOffset),
                21 => new StringEntry(id, relOffset),
                30 => new Int32Entry(id, relOffset),
                31 => new BinaryEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        #region ImageDictionaries
        #region TifTgaImageDictionaries
        public static Entry RawTgaTifTextureDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new BinaryEntry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry TgaTifTextureAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new TgaTifTextureData(id, relOffset),
                19 => new Int32Entry(id, relOffset),
                20 => new StringEntry(id, relOffset),
                21 => new StringEntry(id, relOffset),
                32 => new Int32Entry(id, relOffset),
                33 => new Int32Entry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        #endregion TifTgaImageDictionaries

        #region DDSImageDictionaries

        public static Entry RawDDSTextureDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Image width
                21 => new Int32Entry(id, relOffset),    // Image height
                22 => new BinaryEntry(id, relOffset),   // Raw image data
                23 => new Int32Entry(id, relOffset),    // DDS Format
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry DDSTextureAssetSubTableType1Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ListOfRawDDSTextureData(id, relOffset),
                21 => new Int32Entry(id, relOffset),
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }

        public static Entry DDSTextureAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetSubTableType1(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // File name
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }
        #endregion DDSImageDictionaries

        #region ReflectionMapImageDictionaries
        public static Entry ReflectionMapTextureAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetSubTableType1(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // File name
                // Add more IDs here
                _ => throw new ArgumentException($"Unknown entry ID {id}")
            };
        }
        #endregion ReflectionMapImageDictionaries

        #endregion ImageDictionaries
    }
}
