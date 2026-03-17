using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using Overlord_PackageManager.resources.Data.Factories;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Material
{
    public class BumpedDiffuseMaterial(uint id, uint relOffset, uint typeIdentifier) : AssetEntry(id, relOffset, typeIdentifier)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => MaterialFactory.CreateBumpedDiffuseMaterial;
    }
}