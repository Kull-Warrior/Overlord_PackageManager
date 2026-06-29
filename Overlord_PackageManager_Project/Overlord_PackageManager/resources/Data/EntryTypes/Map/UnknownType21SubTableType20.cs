using Overlord_PackageManager.resources.Data.Factories;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Map
{
    public class UnknownType21SubTableType20(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => MapFactory.CreateUnknownType21SubTableType20;
    }
}
