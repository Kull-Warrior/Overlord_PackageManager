using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif
{
    class TgaTifTextureAsset(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, entryFactory);


            foreach (var entry in Table.Entries)
            {
                if (entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is TgaTifTextureData)
                {
                    List<Int32Entry> intEntries = Table.Entries.OfType<Int32Entry>().ToList();

                    if (intEntries == null)
                        throw new InvalidOperationException("No width and height length found");

                    uint bytesPerPixel = 4;
                    uint rawTextureDataLength = intEntries[0].varInt * intEntries[1].varInt * bytesPerPixel;

                    ((TgaTifTextureData)entry).Read(reader, Table.OffsetOrigin, RawTgaTifTextureDataDictionary, rawTextureDataLength);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateTifHeader(uint width, uint height)
        {
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            {
                uint byteCount = width * height * 4;
                ushort numEntries = 11;
                //8 (Header) + 2 (Tag Count) + (11 * 12 Tags) + 4 (Next IFD Pointer) + 8 (BitsPerSample Data)
                uint pixelDataOffset = 154;
                uint bitsPerSamplePointer = 146;

                // 1. Header
                bw.Write(new char[] { 'I', 'I' }); // Little Endian
                bw.Write((ushort)42);              // Magic Number
                bw.Write((uint)8);                 // Offset to first IFD

                // 2. IFD (Image File Directory)
                bw.Write(numEntries);

                WriteTag(bw, 0x0100, 3, 1, width);               // ImageWidth
                WriteTag(bw, 0x0101, 3, 1, height);              // ImageLength
                WriteTag(bw, 0x0102, 3, 4, bitsPerSamplePointer);// BitsPerSample (Pointer to Array)
                WriteTag(bw, 0x0103, 3, 1, 1);                   // Compression (1 = None)
                WriteTag(bw, 0x0106, 3, 1, 2);                   // Photometric (2 = RGB)
                WriteTag(bw, 0x0111, 4, 1, pixelDataOffset);     // StripOffsets
                WriteTag(bw, 0x0115, 3, 1, 4);                   // SamplesPerPixel (RGBA)
                WriteTag(bw, 0x0116, 3, 1, height);              // RowsPerStrip
                WriteTag(bw, 0x0117, 4, 1, byteCount);           // StripByteCounts
                WriteTag(bw, 0x011C, 3, 1, 1);                   // PlanarConfig (1 = Contiguous)
                WriteTag(bw, 0x0152, 3, 1, 2);                   // ExtraSamples (2 = Alpha)

                bw.Write((uint)0); // Next IFD Pointer (0 = Ende)

                // 3. Values for BitsPerSample (4 x 8 Bit)
                bw.Write((ushort)8);
                bw.Write((ushort)8);
                bw.Write((ushort)8);
                bw.Write((ushort)8);
            }
            return ms.ToArray();
        }

        private void WriteTag(BinaryWriter bw, ushort tag, ushort type, uint count, uint value)
        {
            bw.Write(tag);
            bw.Write(type);
            bw.Write(count);
            bw.Write(value);
        }

        private byte[] CreateTgaHeader(uint width, uint height)
        {
            byte[] header = new byte[18];
            using MemoryStream ms = new MemoryStream(header);
            using BinaryWriter bw = new BinaryWriter(ms);
            {
                bw.Write((byte)0); // ID Length
                bw.Write((byte)0); // Color Map Type
                bw.Write((byte)2); // Image Type (Uncompressed True-Color Image)
                // Color Map Specification (5 bytes)
                for (int i = 0; i < 5; i++)
                    bw.Write((byte)0);
                bw.Write((ushort)0); // X-origin
                bw.Write((ushort)0); // Y-origin
                bw.Write((ushort)width);  // Image Width
                bw.Write((ushort)height); // Image Height
                bw.Write((byte)32); // Bits per pixel
                bw.Write((byte)8); // Image Descriptor (8 bits of alpha)
            }
            return header;
        }

        public void WriteToFile(string baseDir)
        {
            byte[] header;
            string filePath = "";
            List<StringEntry> tgaTifAssetStrings = Table.Entries.OfType<StringEntry>().ToList();
            string fileName = tgaTifAssetStrings[1].varString;

            List<Int32Entry> ints = Table.Entries.OfType<Int32Entry>().ToList();
            uint width = ints[0].varInt;
            uint height = ints[1].varInt;

            TgaTifTextureData dataContainer = (TgaTifTextureData)Table.Entries[5];
            byte[] rawRGBA = ((BinaryEntry)dataContainer.Table.Entries[0]).varBytes;

            if (fileName.ToLower().EndsWith(".tif"))
            {
                Directory.CreateDirectory(baseDir + "\\TIFF");
                filePath = baseDir + "\\TIFF\\" + fileName;
                header = CreateTifHeader(width, height);

            }
            else if (fileName.ToLower().EndsWith(".tga"))
            {
                Directory.CreateDirectory(baseDir + "\\TGA");
                filePath = baseDir + "\\TGA\\" + fileName;
                header = CreateTgaHeader(width, height);
            }
            else
            {
                throw new InvalidDataException("Unsupported file format for TgaTifTextureAsset: " + fileName);
            }

            using FileStream fs = File.Open(filePath, FileMode.Create);
            using BinaryWriter br = new BinaryWriter(fs);
            {
                br.Write(header);
                br.Write(rawRGBA);
            }
        }
    }
}
