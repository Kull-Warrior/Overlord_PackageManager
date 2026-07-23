using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth;
using Overlord_PackageManager.resources.Data.EntryTypes.Resource;
using Overlord_PackageManager.resources.Data.EntryTypes.XML;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class ResourcePackFactory
    {
        public static Entry CreateNamedAssetContainer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),     // Name of the group
                //21 => new AssetList(id, relOffset),       // Asset list
                21 => new AssetListContainer(id, relOffset),       // Asset list
                30 => new TableEntry(id, relOffset),        // Unknown table
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte)
            };
        }

        public static Entry CreateResourcePackRootTable(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                16 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry     // Unknown
                17 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry     // Unknown
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                22 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),       // Character/Rescource Name
                23 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains Effects and Light Data of some sort
                24 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains "TS", Texture related Data
                25 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains "0" Type Data Unknown use, Map editor friendly apperently
                26 => new AssetListContainer(id, relOffset),         // Contains Object, Mesh, Material, Texture, SFX, Animation and Shader Data
                27 => new CharListCountedArrayEntry(id, relOffset),  // Contains a list of strings. Reference to one or more .CLB files.
                28 => new XMLEntry(id, relOffset),          // Contains XML Data
                29 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry     // Unknown
                30 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains "2" Type Data Unknown use, contains lua scripts
                31 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains Animation Data
                32 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Unknown
                33 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains Object, Mesh Data //MapEditor Objects apperently
                34 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains Object Data, Animation Data
                35 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Unknown
                36 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Unknown
                37 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains SFX Data
                38 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains FXE Data ( Raw file)
                39 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // 
                40 => new NamedAssetContainer(id, relOffset),   // Unknown entry
                41 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                42 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                43 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                60 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                61 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                62 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                63 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                64 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                65 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                66 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                67 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                68 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                69 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                70 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                71 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                72 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                73 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                74 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                75 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                77 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                78 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                80 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                81 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                82 => new XMLEntry(id, relOffset),          // Contains XML Data (System_Quests)
                83 => new XMLEntry(id, relOffset),          // Contains XML Data (Tower_GeneralMessages)
                84 => new XMLEntry(id, relOffset),          // Contains XML Data (TutorialMessages_Mouse)
                85 => new XMLEntry(id, relOffset),          // Contains XML Data (TutorialMessages_Gamepad)
                86 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry     // Contains CPT_ Data
                87 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry     // Contains CPTX Data
                88 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry     // "Contains a string. with : 0D = Carriage Return(CR) 0A = Line Feed(LF) to break lines. Referencing here the multiplayer maps"
                89 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                90 => new XMLEntry(id, relOffset),          // Contains XML Data (Credits)
                91 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}