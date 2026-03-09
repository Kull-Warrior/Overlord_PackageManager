using Overlord_PackageManager.resources.EntryTypes;
using Overlord_PackageManager.resources.EntryTypes.Animation;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.EntryTypes.Lua;
using Overlord_PackageManager.resources.EntryTypes.XML;
using System.IO;

namespace Overlord_PackageManager.resources.Generic
{
    public abstract class Entry
    {
        public uint Id;
        public uint RelativeOffset;
        // Absolute payload size of this entry
        public long PayloadLength;

        protected Entry()
        {

        }

        protected Entry(uint id, uint relOffset)
        {
            Id = id;
            RelativeOffset = relOffset;
        }

        // Each entry knows how to read itself
        public abstract void Read(BinaryReader reader, long origin);

        public static Entry InfoTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),
                21 => new Int32Entry(id, relOffset),
                22 => new BlobEntry(id, relOffset),   // Unknown entry
                30 => new StringEntry(id, relOffset),
                31 => new Int32Entry(id, relOffset),
                32 => new BlobEntry(id, relOffset),
                33 => new BlobEntry(id, relOffset),
                36 => new Int32Entry(id, relOffset),
                37 => new Int32Entry(id, relOffset),
                38 => new Int32Entry(id, relOffset),
                39 => new Int32Entry(id, relOffset),
                40 => new StringEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry OMPDataRootTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new TerrainDataEntry(id, relOffset),
                21 => new UnknownTableType21Entry(id, relOffset),
                22 => new BlobEntry(id, relOffset),
                23 => new BlobEntry(id, relOffset),
                24 => new BlobEntry(id, relOffset),
                25 => new LuaListEntry(id, relOffset),
                26 => new BlobEntry(id, relOffset),   // Unknown entry
                27 => new BlobEntry(id, relOffset),   // Unknown entry
                28 => new BlobEntry(id, relOffset),   // Unknown entry
                29 => new SingleByteEntry(id, relOffset),
                30 => new BlobEntry(id, relOffset),   // Unknown entry
                31 => new BlobEntry(id, relOffset),
                32 => new AssetListContainer(id, relOffset),
                33 => new LuaEntry(id, relOffset),
                34 => new StringEntry(id, relOffset),
                35 => new BlobEntry(id, relOffset),   // Unknown entry
                36 => new LuaEntry(id, relOffset),
                37 => new FloatEntry(id, relOffset),
                38 => new FloatEntry(id, relOffset),
                39 => new FloatEntry(id, relOffset),
                40 => new SingleByteEntry(id, relOffset),
                41 => new SingleByteEntry(id, relOffset),
                42 => new Int32Entry(id, relOffset),
                43 => new Int32Entry(id, relOffset),
                45 => new LuaEntry(id, relOffset),
                46 => new StringListEntry(id, relOffset),
                47 => new BlobEntry(id, relOffset),   // Unknown entry
                48 => new BlobEntry(id, relOffset),   // Unknown entry
                49 => new Int32Entry(id, relOffset),
                51 => new Int32Entry(id, relOffset),
                52 => new BlobEntry(id, relOffset),   // Unknown entry
                53 => new BlobEntry(id, relOffset),   // Unknown entry
                100 => new BlobEntry(id, relOffset),   // Unknown entry
                101 => new BlobEntry(id, relOffset),   // Unknown entry
                102 => new BlobEntry(id, relOffset),   // Unknown entry
                103 => new BlobEntry(id, relOffset),   // Unknown entry
                104 => new Int32Entry(id, relOffset),
                106 => new SingleByteEntry(id, relOffset),
                108 => new SingleByteEntry(id, relOffset),
                110 => new Int32Entry(id, relOffset),
                111 => new Int32Entry(id, relOffset),
                112 => new Int32Entry(id, relOffset),
                113 => new Int32Entry(id, relOffset),
                114 => new StringEntry(id, relOffset),
                115 => new Int32Entry(id, relOffset),
                116 => new SingleByteEntry(id, relOffset),
                117 => new SingleByteEntry(id, relOffset),
                120 => new BlobEntry(id, relOffset),   // Unknown entry
                121 => new Int32Entry(id, relOffset),
                122 => new LuaEntry(id, relOffset),
                123 => new LuaEntry(id, relOffset),
                124 => new BlobEntry(id, relOffset),   // Unknown entry
                125 => new Int32Entry(id, relOffset),
                126 => new Int32Entry(id, relOffset),
                127 => new LuaEntry(id, relOffset),
                128 => new Int32Entry(id, relOffset),
                129 => new Int32Entry(id, relOffset),
                130 => new BlobEntry(id, relOffset),   // Unknown entry
                131 => new BlobEntry(id, relOffset),   // Unknown entry
                132 => new BlobEntry(id, relOffset),
                133 => new BlobEntry(id, relOffset),   // Unknown entry
                134 => new StringEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry TerrainDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),
                31 => new Int32Entry(id, relOffset),
                33 => new BlobEntry(id, relOffset),
                34 => new StringEntry(id, relOffset),
                35 => new StringEntry(id, relOffset),
                36 => new FloatEntry(id, relOffset),
                37 => new BlobEntry(id, relOffset),
                38 => new SingleByteEntry(id, relOffset),
                39 => new BlobEntry(id, relOffset),
                40 => new SingleByteEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry UnknownType21Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new BlobEntry(id, relOffset),   // Unknown entry
                21 => new Int32Entry(id, relOffset),
                22 => new SingleByteEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry UnknownType21SubTableType20Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),
                31 => new StringEntry(id, relOffset),
                32 => new BlobEntry(id, relOffset),   // Unknown entry
                34 => new BlobEntry(id, relOffset),   // Unknown entry
                35 => new Int32Entry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry GameObjectDataSubTableType20SubTableType32Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                10 => new Int32Entry(id, relOffset),
                11 => new Int32Entry(id, relOffset),
                12 => new StringEntry(id, relOffset),
                20 => new BlobEntry(id, relOffset),
                21 => new BlobEntry(id, relOffset),
                22 => new Int32Entry(id, relOffset),
                23 => new Int32Entry(id, relOffset),
                24 => new Int32Entry(id, relOffset),
                40 => new SingleByteEntry(id, relOffset),
                42 => new SingleByteEntry(id, relOffset),
                60 => new StringEntry(id, relOffset),
                61 => new BlobEntry(id, relOffset),   // Unknown entry
                62 => new BlobEntry(id, relOffset),   // Unknown entry
                63 => new SingleByteEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry DataSubTableType21SubTableType20SubTableType34Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                22 => new Int32Entry(id, relOffset),
                23 => new Int32ArrayEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry LuaDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new StringEntry(id, relOffset),
                21 => new StringListEntry(id, relOffset),
                22 => new Int32Entry(id, relOffset),
                23 => new BlobEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
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
                12 => new BlobEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry ResourcePackLinkDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),
                31 => new StringEntry(id, relOffset),
                32 => new StringEntry(id, relOffset),
                33 => new SingleByteEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry ResourcePackRootTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                16 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                17 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                19 => new Int32Entry(id, relOffset),
                20 => new Int32Entry(id, relOffset),
                22 => new StringEntry(id, relOffset),       // Character/Rescource Name
                23 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains Effects and Light Data of some sort
                24 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains "TS", Texture related Data
                25 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains "0" Type Data Unknown use, Map editor friendly apperently
                26 => new AssetListContainer(id, relOffset),         // Contains Object, Mesh, Material, Texture, SFX, Animation and Shader Data
                27 => new StringListEntry(id, relOffset),  // Contains a list of strings. Reference to one or more .CLB files.
                28 => new XMLEntry(id, relOffset),          // Contains XML Data
                29 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                30 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains "2" Type Data Unknown use, contains lua scripts
                31 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains Animation Data
                32 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                33 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains Object, Mesh Data //MapEditor Objects apperently
                34 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains Object Data, Animation Data
                35 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                36 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                37 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains SFX Data
                38 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains FXE Data ( Raw file)
                39 => new BlobEntry(id, relOffset),   // Unknown entry     // 
                40 => new BlobEntry(id, relOffset),   // Unknown entry
                41 => new BlobEntry(id, relOffset),   // Unknown entry
                42 => new BlobEntry(id, relOffset),   // Unknown entry
                43 => new BlobEntry(id, relOffset),   // Unknown entry
                60 => new BlobEntry(id, relOffset),   // Unknown entry
                61 => new BlobEntry(id, relOffset),   // Unknown entry
                62 => new BlobEntry(id, relOffset),   // Unknown entry
                63 => new BlobEntry(id, relOffset),   // Unknown entry
                64 => new BlobEntry(id, relOffset),   // Unknown entry
                65 => new BlobEntry(id, relOffset),   // Unknown entry
                66 => new BlobEntry(id, relOffset),   // Unknown entry
                67 => new BlobEntry(id, relOffset),   // Unknown entry
                68 => new BlobEntry(id, relOffset),   // Unknown entry
                69 => new BlobEntry(id, relOffset),   // Unknown entry
                70 => new BlobEntry(id, relOffset),   // Unknown entry
                71 => new BlobEntry(id, relOffset),   // Unknown entry
                72 => new BlobEntry(id, relOffset),   // Unknown entry
                73 => new BlobEntry(id, relOffset),   // Unknown entry
                74 => new BlobEntry(id, relOffset),   // Unknown entry
                75 => new BlobEntry(id, relOffset),   // Unknown entry
                77 => new BlobEntry(id, relOffset),   // Unknown entry
                78 => new BlobEntry(id, relOffset),   // Unknown entry
                80 => new BlobEntry(id, relOffset),   // Unknown entry
                81 => new BlobEntry(id, relOffset),   // Unknown entry
                82 => new XMLEntry(id, relOffset),          // Contains XML Data (System_Quests)
                83 => new XMLEntry(id, relOffset),          // Contains XML Data (Tower_GeneralMessages)
                84 => new XMLEntry(id, relOffset),          // Contains XML Data (TutorialMessages_Mouse)
                85 => new XMLEntry(id, relOffset),          // Contains XML Data (TutorialMessages_Gamepad)
                86 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains CPT_ Data
                87 => new BlobEntry(id, relOffset),   // Unknown entry     // Contains CPTX Data
                88 => new BlobEntry(id, relOffset),   // Unknown entry     // "Contains a string. with : 0D = Carriage Return(CR) 0A = Line Feed(LF) to break lines. Referencing here the multiplayer maps"
                89 => new BlobEntry(id, relOffset),   // Unknown entry
                90 => new XMLEntry(id, relOffset),          // Contains XML Data (Credits)
                91 => new BlobEntry(id, relOffset),   // Unknown entry
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry RPKDataRootGenericSubTableDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new StringEntry(id, relOffset),
                21 => new BlobEntry(id, relOffset),   // Unknown entry
                30 => new BlobEntry(id, relOffset),   // Unknown entry
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry AssetListContainerDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new AssetList(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        #region AnimationDictionaries

        public static Entry BoneAnimationSubTableType22Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Unkown u32
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry BoneAnimationSubTableType23Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Unkown u32
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry BoneAnimationSubTableType24Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),                // Unkown u32
                21 => new Int32Entry(id, relOffset),                // Number of Bone positions, if the bone does not move in the animation only a single entry can be found here
                22 => new BonePositionDataArray(id, relOffset),     // Array of Bone positions
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry BoneAnimationSubTableType25SubTableType21Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                22 => new Int32Entry(id, relOffset),                // Number of Bone rotations
                23 => new BoneRotationDataArray(id, relOffset),     // Array of Bone rotations
                24 => new BlobEntry(id, relOffset),               // Unkown 12 Bytes
                30 => new Int32Entry(id, relOffset),                // Number of Bone scales
                31 => new BoneScaleDataArray(id, relOffset),        // Number of Bone scales
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry BoneAnimationSubTableType25Dictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),                                    // Unkown u32
                21 => new BoneAnimationSubTableType25SubTableType21(id, relOffset),     // Contains Bone Rotation and Scale data
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry BoneAnimationDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new StringEntry(id, relOffset),                   // Bone Name
                21 => new Int64Entry(id, relOffset),                    // Unkown u64
                22 => new BoneAnimationSubTableType22(id, relOffset),   // Unkown use
                23 => new BoneAnimationSubTableType23(id, relOffset),   // Unkown use
                24 => new BoneAnimationSubTableType24(id, relOffset),   // Contains Bone Position data at deeper levels
                25 => new BoneAnimationSubTableType25(id, relOffset),   // Contains Bone Rotation and Scale data at deeper levels
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry AnimationAssetDataContainerDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                10 => new AssetListContainer(id, relOffset),     // List of all bone animations making up the entire animation, meaning each bone and its corresponding animation data
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),      // Unknown entry
            };
        }

        public static Entry AnimationAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                01 => new AnimationAssetDataContainer(id, relOffset),   // Sub reference table containing a list of Bone Animation Data
                19 => new Int32Entry(id, relOffset),                    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),                   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),                   // Animation name
                30 => new FloatEntry(id, relOffset),                    // Unkown float and unknown use
                31 => new Int64Entry(id, relOffset),                    // unkown u64 and unknown use
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        #endregion AnimationDictionaries


        #region SFXDictionaries

        public static Entry SFXDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new Int32Entry(id, relOffset),    // Length of SFX Data
                31 => new BlobEntry(id, relOffset),    // SFX Data, Header + Raw Audio Data, full wav style file
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry SFXAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new SFXData(id, relOffset),        // Sub reference table containing a int32 and a full wav style file
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // Sound name
                100 => new StringEntry(id, relOffset),  // File name
                101 => new Int32Entry(id, relOffset),   // FFFF Block unkown use
                104 => new Int32Entry(id, relOffset),    // Unkown int32
                105 => new SingleByteEntry(id, relOffset),    // Unkown single byte
                106 => new Int32Entry(id, relOffset),   // Unkown int32
                107 => new Int32Entry(id, relOffset),   // Unkown int32
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        #endregion SFXDictionaries

        #region ImageDictionaries
        #region TifTgaImageDictionaries
        public static Entry RawTgaTifTextureDataDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                30 => new BlobEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
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
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        #endregion TifTgaImageDictionaries

        #region DDSImageDictionaries

        public static Entry DDSTextureDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new Int32Entry(id, relOffset),    // Image width
                21 => new Int32Entry(id, relOffset),    // Image height
                22 => new BlobEntry(id, relOffset),   // Raw image data
                23 => new Int32Entry(id, relOffset),    // DDS Format
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry DDSTextureAssetDataContainerDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                20 => new AssetListContainer(id, relOffset),     // List of all dds images making up the entire dds file, meaning the main image and each mipmap
                21 => new Int32Entry(id, relOffset),     // Unkown
                23 => new Int32Entry(id, relOffset),     // Unkown
                24 => new BlobEntry(id, relOffset),   // Unknown entry
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry DDSTextureAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetDataContainer(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // File name
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
        #endregion DDSImageDictionaries

        #region ReflectionCubeMapDictionaries
        public static Entry ReflectionCubeMapAssetDictionary(uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetDataContainer(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new Int32Entry(id, relOffset),    // FFFF Block unkown use
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // File name
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
        #endregion ReflectionCubeMapDictionaries

        #endregion ImageDictionaries
    }
}
