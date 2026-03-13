using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.EntryTypes.Lua;
using Overlord_PackageManager.resources.Data.EntryTypes.Map;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class MapFactory
    {
        public static Entry CreateUnknownType21(BinaryReader reader, uint id, uint relOffset)
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

        public static Entry CreateUnknownType21SubTableType20(BinaryReader reader, uint id, uint relOffset)
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

        public static Entry CreateGameObjectDataSubTableType20SubTableType32(BinaryReader reader, uint id, uint relOffset)
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

        public static Entry CreateDataSubTableType21SubTableType20SubTableType34(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                22 => new Int32Entry(id, relOffset),
                23 => new Int32ArrayEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateInfoTable(BinaryReader reader, uint id, uint relOffset)
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

        public static Entry CreateRootTable(BinaryReader reader, uint id, uint relOffset)
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
    }
}