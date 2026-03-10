using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    public class RootEntry : TableEntry
    {
        public RootEntry() : base(0, 0) { }

        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => Entry.ResourcePackRootTableDictionary;
    }
}
