using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public class DDSTextureAsset(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
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
                if(entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is DDSTextureAssetDataContainer)
                {
                    ((DDSTextureAssetDataContainer)entry).Read(reader, Table.OffsetOrigin, DDSTextureAssetDataContainerDictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public void WriteToFile(string baseDir)
        {
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            string fileName = ((StringEntry)Table.Entries[1]).varString;
            if (!fileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".dds";
            }

            DDSTextureAssetDataContainer container = (DDSTextureAssetDataContainer)Table.Entries[3];

            ListOfDDSTextures list = (ListOfDDSTextures)container.Table.Entries[0];

            List<DDSTextures> textures = list.Table.Entries.OfType<DDSTextures>().ToList();

            if (textures.Count == 0)
            {
                return;
            }

            uint width = ((Int32Entry)textures[0].Table.Entries[0]).varInt;
            uint height = ((Int32Entry)textures[0].Table.Entries[1]).varInt;
            DDSFormat format = (DDSFormat)((Int32Entry)textures[0].Table.Entries[2]).varInt;

            uint mipCount = DDSWriter.CalculateMipMapCount(width, height);

            byte[] header = DDSWriter.CreateDDSHeader(width, height, mipCount, format, false);

            using FileStream fs = File.Create(Path.Combine(baseDir, fileName));
            using BinaryWriter bw = new BinaryWriter(fs);
            {
                bw.Write(header);

                // Standard 2D texture: just write sequential mips
                foreach (DDSTextures tex in textures)
                {
                    BlobEntry blob = tex.Table.Entries.OfType<BlobEntry>().First();
                    bw.Write(blob.varBytes);
                }
            }
        }
    }
}
