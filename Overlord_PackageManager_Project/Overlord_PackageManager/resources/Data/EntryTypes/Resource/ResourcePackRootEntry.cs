using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Resource
{
    public class ResourcePackRootEntry : TableEntry
    {
        public ResourcePackRootEntry() : base(0, 0) { }

        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => ResourcePackRootTableDictionary;
    }
}
