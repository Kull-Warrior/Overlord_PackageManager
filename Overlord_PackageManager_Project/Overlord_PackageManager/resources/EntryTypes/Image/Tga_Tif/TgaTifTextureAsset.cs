using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif
{
    class TgaTifTextureAsset(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] identifier;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            identifier = reader.ReadBytes(4);
            varRefTable = new RefTable(reader, entryFactory);


            foreach (var entry in varRefTable.Entries)
            {
                if(entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is TgaTifTextureData)
                {
                    List<Int32Entry> intEntries = varRefTable.Entries.OfType<Int32Entry>().ToList();
                    List<Int32Entry> lastTwoIntEntries = intEntries.TakeLast(2).ToList();

                    if (lastTwoIntEntries == null)
                        throw new InvalidOperationException("No width and height length found");

                    uint bytesPerPixel = 4;
                    uint rawTextureDataLength = lastTwoIntEntries[0].varInt * lastTwoIntEntries[1].varInt * bytesPerPixel;

                    ((TgaTifTextureData)entry).Read(reader, varRefTable.origin, RawTifTgaTextureDataDictionary, rawTextureDataLength);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
