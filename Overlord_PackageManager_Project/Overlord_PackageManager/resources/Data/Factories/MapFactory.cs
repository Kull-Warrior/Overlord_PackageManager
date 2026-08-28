using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth;
using Overlord_PackageManager.resources.Data.EntryTypes.Lua;
using Overlord_PackageManager.resources.Data.EntryTypes.Map;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class MapFactory
    {
        public static Entry CreateWorldEntityPackage(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new EntityAllocationTable(id, relOffset),   // Unknown entry
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                22 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateEntityAllocationTable(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new AssetList(id, relOffset),     // Data
                30 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                31 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                32 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                34 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                35 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateDataSubTableType21SubTableType20SubTableType34(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                22 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                23 => new CountedArrayEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateMapBuildInformation(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                22 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                23 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                24 => new ScalarEntry<ulong>(id, relOffset, BinaryTypes.UInt64),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateInfoTable(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                22 => new MapBuildInformation(id, relOffset),   // Unknown entry
                30 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                31 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                32 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                33 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                36 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                37 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                38 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                39 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                40 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateRootTable(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new TerrainDataEntry(id, relOffset),
                21 => new WorldEntityPackage(id, relOffset),
                22 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),           // Player/Camera Start Location ???
                23 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),           // Player/Camera rotation ???
                24 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),           // Player/Camera rotation ???
                25 => new AssetListContainer(id, relOffset),                                // Contains Assets of Type 95000004, which are LuaAssets
                26 => new TableEntry(id, relOffset),   // Unknown entry
                27 => new TableEntry(id, relOffset),   // Unknown entry
                28 => new TableEntry(id, relOffset),   // Unknown entry
                29 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                30 => new AssetListContainer(id, relOffset),                                // Contains Assets of Type 0000004
                31 => new TableEntry(id, relOffset),
                32 => new AssetListContainer(id, relOffset),                                // Contains Assets of Type 1E000004, which seems to be a list of to be imported resource files with a priority?
                33 => new LuaEntry(id, relOffset),                                          // Contains a pre-load lua script for this map
                34 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                35 => new AssetListContainer(id, relOffset),                                // Contains Assets of Type BA000004
                36 => new LuaEntry(id, relOffset),                                          // Contains a post-load lua script for this map
                37 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                38 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                39 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                40 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                41 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                42 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                43 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                45 => new LuaEntry(id, relOffset),                                          // Contains a lua script for the conditional loading of RPK-Files, Post ResourcePack Load Script
                46 => new CharListCountedArrayEntry(id, relOffset),
                47 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                48 => new TableEntry(id, relOffset),   // Unknown entry
                49 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                51 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                52 => new TableEntry(id, relOffset),   // Unknown entry
                53 => new TableEntry(id, relOffset),   // Unknown entry
                100 => new AssetListContainer(id, relOffset),                               // Contains Assets of Type 64004600
                101 => new TableEntry(id, relOffset),   // Unknown entry
                102 => new AssetListContainer(id, relOffset),                               // Contains Assets of Type 73004600
                103 => new AssetListContainer(id, relOffset),                               // Contains Assets of Type 3B074600
                104 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                106 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                108 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                110 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                111 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                112 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                113 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                114 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                115 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                116 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                117 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                120 => new AssetListContainer(id, relOffset),                               // Contains Assets of 53064600 
                121 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                122 => new LuaEntry(id, relOffset),                                         // Contains a lua script for Title Scripting and starts the Title Camera Sequence ? At least in the tower. 
                123 => new LuaEntry(id, relOffset),                                         // Contains a lua script for on Entry Cut Scenes & Quest Logic
                124 => new AssetListContainer(id, relOffset),                               // Contains Assets of Type 9F004600
                125 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                126 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                127 => new LuaEntry(id, relOffset),                                         // Contains a lua script for setting the Environment and Simulation Scripting
                128 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                129 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                130 => new TableEntry(id, relOffset),   // Unknown entry
                131 => new TableEntry(id, relOffset),   // Unknown entry
                132 => new TableEntry(id, relOffset),
                133 => new LuaEntry(id, relOffset),   // Unknown entry
                134 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),              // Unknown entry
            };
        }
    }
}