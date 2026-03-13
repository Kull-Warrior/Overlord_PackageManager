using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                20 => new StringEntry(id, relOffset),     // Name of the group
                //21 => new AssetList(id, relOffset),       // Asset list
                21 => new AssetListContainer(id, relOffset),       // Asset list
                30 => new BlobEntry(id, relOffset),       // Unknown table
                _ => new BlobEntry(id, relOffset)
            };
        }

        public static Entry CreateResourcePackRootTable(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                16 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                17 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
                19 => new Int32Entry(id, relOffset),
                20 => new Int32Entry(id, relOffset),
                22 => new StringEntry(id, relOffset),       // Character/Rescource Name
                23 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains Effects and Light Data of some sort
                24 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains "TS", Texture related Data
                25 => new NamedAssetContainer(id, relOffset),   // Unknown entry     // Contains "0" Type Data Unknown use, Map editor friendly apperently
                26 => new AssetListContainer(id, relOffset),         // Contains Object, Mesh, Material, Texture, SFX, Animation and Shader Data
                27 => new StringListEntry(id, relOffset),  // Contains a list of strings. Reference to one or more .CLB files.
                28 => new XMLEntry(id, relOffset),          // Contains XML Data
                29 => new BlobEntry(id, relOffset),   // Unknown entry     // Unknown
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
    }
}