using Overlord_PackageManager.resources.Data.Factories;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset
{
    class UnkownAssetType1F004600(uint id, uint relOffset, uint typeIdentifier) : AssetEntry(id, relOffset, typeIdentifier)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => UnkownAssetFactory.CreateUnkownAssetType1F004600;
    }
}
