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
                22 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),           // Player/Camera Start Location
                23 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),           // Player/Camera rotation
                24 => new RawArrayEntry<float>(id, relOffset, BinaryTypes.Float),           // Player/Camera rotation
                25 => new AssetListContainer(id, relOffset),
                26 => new TableEntry(id, relOffset),   // Unknown entry
                27 => new TableEntry(id, relOffset),   // Unknown entry
                28 => new TableEntry(id, relOffset),   // Unknown entry
                29 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                30 => new TableEntry(id, relOffset),   // Unknown entry
                31 => new TableEntry(id, relOffset),
                32 => new AssetListContainer(id, relOffset),
                33 => new LuaEntry(id, relOffset),
                34 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                35 => new TableEntry(id, relOffset),   // Unknown entry
                36 => new LuaEntry(id, relOffset),
                37 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                38 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                39 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                40 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                41 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                42 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                43 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                45 => new LuaEntry(id, relOffset),
                46 => new CharListCountedArrayEntry(id, relOffset),
                47 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                48 => new TableEntry(id, relOffset),   // Unknown entry
                49 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                51 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                52 => new TableEntry(id, relOffset),   // Unknown entry
                53 => new TableEntry(id, relOffset),   // Unknown entry
                100 => new TableEntry(id, relOffset),   // Unknown entry
                101 => new TableEntry(id, relOffset),   // Unknown entry
                102 => new TableEntry(id, relOffset),   // Unknown entry
                103 => new TableEntry(id, relOffset),   // Unknown entry
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
                120 => new TableEntry(id, relOffset),   // Unknown entry
                121 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                122 => new LuaEntry(id, relOffset),
                123 => new LuaEntry(id, relOffset),
                124 => new TableEntry(id, relOffset),   // Unknown entry
                125 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                126 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                127 => new LuaEntry(id, relOffset),
                128 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                129 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                130 => new TableEntry(id, relOffset),   // Unknown entry
                131 => new TableEntry(id, relOffset),   // Unknown entry
                132 => new TableEntry(id, relOffset),
                133 => new LuaEntry(id, relOffset),   // Unknown entry
                134 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}