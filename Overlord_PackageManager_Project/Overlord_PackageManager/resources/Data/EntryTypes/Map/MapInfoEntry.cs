using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Map
{
    public class MapInfoEntry : TableEntry
    {
        public MapInfoEntry() : base(0, 0) { }

        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => OMPDataRootTableDictionary;
    }
}
