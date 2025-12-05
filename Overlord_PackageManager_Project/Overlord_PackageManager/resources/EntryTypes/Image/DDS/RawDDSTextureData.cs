using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    class RawDDSTextureData(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public uint entryTypeIdentifier;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            entryTypeIdentifier = reader.ReadUInt32();
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                if (entry is Int32Entry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is BinaryEntry)
                {
                    List<Int32Entry> intEntries = varRefTable.Entries.OfType<Int32Entry>().ToList();

                    List<Int32Entry> lastThree = intEntries.TakeLast(3).ToList();

                    if (lastThree == null)
                        throw new InvalidOperationException("No ByteCode length found");
                    
                    uint width = lastThree[0].varInt;
                    uint height = lastThree[1].varInt;
                    uint format = lastThree[2].varInt;

                    uint rawByteLength = 0;
                    uint blockSize = 0;
                    uint blocksWidth = 0;
                    uint blocksHeight = 0;

                    if (format == 7 || format == 11 || format == 9)
                    {
                        if (format == 7)
                        {
                            blockSize = 8;
                        }
                        else
                        {
                            blockSize = 16;
                        }

                        blocksWidth = (width + 3u) / 4u;
                        blocksHeight = (height + 3u) / 4u;

                        rawByteLength = blocksWidth * blocksHeight * blockSize;
                    }
                    else
                    {
                        rawByteLength = width * height;
                    }

                    ((BinaryEntry)entry).Read(reader, varRefTable.origin, rawByteLength);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
