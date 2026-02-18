using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    class RawDDSTextureData(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;


        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is Int32Entry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is BinaryEntry)
                {
                    List<Int32Entry> intEntries = Table.Entries.OfType<Int32Entry>().ToList();

                    List<Int32Entry> lastThree = intEntries.TakeLast(3).ToList();

                    if (lastThree == null)
                        throw new InvalidOperationException("No ByteCode length found");
                    
                    uint width = lastThree[0].varInt;
                    uint height = lastThree[1].varInt;
                    uint rawFormat = lastThree[2].varInt;
                    DDSFormat format = (DDSFormat)rawFormat;

                    uint rawByteLength = 0;
                    uint blockSize = 0;
                    uint blocksWidth = 0;
                    uint blocksHeight = 0;

                    if (format == DDSFormat.DXT1 || format == DDSFormat.DXT3 || format == DDSFormat.DXT5)
                    {
                        if (format == DDSFormat.DXT1)
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
                    else if (format == DDSFormat.UncompressedRGB)
                    {
                        rawByteLength = width * height * 3;
                    }
                    else if (format == DDSFormat.UncompressedRGBA)
                    {
                        rawByteLength = width * height * 4;
                    }
                    else
                    {
                        throw new NotImplementedException("Unkown Image Format : " + rawFormat);
                    }

                    ((BinaryEntry)entry).Read(reader, Table.OffsetOrigin, rawByteLength);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
