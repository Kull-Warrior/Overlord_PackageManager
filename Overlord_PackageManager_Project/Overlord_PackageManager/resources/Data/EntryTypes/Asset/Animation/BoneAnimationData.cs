using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation
{
    class BoneAnimationData(uint id, uint relOffset, uint typeIdentifier) : AssetEntry(id, relOffset, typeIdentifier)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => BoneAnimationDataDictionary;
    }
}
