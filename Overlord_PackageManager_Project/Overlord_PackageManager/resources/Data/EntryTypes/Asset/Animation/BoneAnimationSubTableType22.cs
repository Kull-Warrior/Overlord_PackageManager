using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation
{
    class BoneAnimationSubTableType22(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => BoneAnimationSubTableType22Dictionary;
    }
}
