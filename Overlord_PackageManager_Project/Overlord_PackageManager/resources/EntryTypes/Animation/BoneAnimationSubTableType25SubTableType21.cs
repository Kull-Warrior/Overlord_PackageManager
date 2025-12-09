using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneAnimationSubTableType25SubTableType21(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                if (entry is Int32Entry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is BinaryEntry)
                {
                    ((BinaryEntry)entry).Read(reader, varRefTable.origin, 12);
                }
                if (entry is BoneRotationDataArray)
                {
                    List<Int32Entry> intEntries = varRefTable.Entries.OfType<Int32Entry>().ToList();

                    if (intEntries == null)
                        throw new InvalidOperationException("No ByteCode length found");
                    
                    ((BoneRotationDataArray)entry).Read(reader, varRefTable.origin, intEntries[0].varInt);
                }
                if (entry is BoneScaleDataArray)
                {
                    List<Int32Entry> intEntries = varRefTable.Entries.OfType<Int32Entry>().ToList();

                    if (intEntries == null)
                        throw new InvalidOperationException("No ByteCode length found");

                    ((BoneScaleDataArray)entry).Read(reader, varRefTable.origin, intEntries[1].varInt);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
