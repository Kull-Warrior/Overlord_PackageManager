using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    internal class UnkownAssetFactory
    {
        public static Entry CreateUnkownAssetType09004600SubTableType32(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new AssetList(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType09004600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                30 => new UInt32Entry(id, relOffset),    // Index?
                31 => new StringEntry(id, relOffset),
                32 => new AssetListContainer(id, relOffset),
                33 => new UInt32Entry(id, relOffset),
                34 => new TableEntry(id, relOffset),
                35 => new UInt32Entry(id, relOffset),    // Parent Index?
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType08464600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType13004600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType14464600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType15004600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType15304600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType1F004600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateUnkownAssetType20464600(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}