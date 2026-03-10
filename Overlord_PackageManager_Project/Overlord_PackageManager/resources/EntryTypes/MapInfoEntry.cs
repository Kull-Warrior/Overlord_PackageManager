using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    public class MapInfoEntry : TableEntry
    {
        public MapInfoEntry() : base(0, 0) { }

        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => Entry.OMPDataRootTableDictionary;
    }
}
