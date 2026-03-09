using System.IO;

namespace Overlord_PackageManager.resources.Generic
{
    public class TableEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table { get; set; }

        public ReferenceTable GetReferenceTable() => Table;

        // Grammar definition for this table type
        protected virtual Func<BinaryReader, uint, uint, Entry> EntryFactory => null;

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
