using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Resource;
using Overlord_PackageManager.resources.Data.EntryTypes.XML;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.Data.Files.RPK
{
    public class ResourcePackHeader
    {
        public string Magic = "";          // should be "RPK\0"
        public uint Version;
        public uint FileId;
        public uint TotalDataSize;
        public string Name = "";

        public ResourcePackHeader(BinaryReader br)
        {
            Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            Version = br.ReadUInt32();
            FileId = br.ReadUInt32();
            TotalDataSize = br.ReadUInt32();
            byte[] nameBytes = br.ReadBytes(160);
            Name = Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Magic));
            bw.Write(Version);
            bw.Write(FileId);
            bw.Write(TotalDataSize);

            byte[] nameBytes = new byte[160];

            byte[] encoded = Encoding.ASCII.GetBytes(Name);
            Array.Copy(encoded, nameBytes, Math.Min(encoded.Length, 160));

            bw.Write(nameBytes);
        }
    }
    public class ResourcePackBody
    {
        public ResourcePackRootEntry Data;
    }

    public class ResourcePackFile
    {
        public ResourcePackHeader Header;
        public ResourcePackBody Body;
        public void Read(string path)
        {
            try
            {
                using FileStream fs = File.OpenRead(path);
                using BinaryReader br = new BinaryReader(fs);
                {
                    Header = new ResourcePackHeader(br);
                    Body = new ResourcePackBody();

                    long origin = br.BaseStream.Position;

                    Body.Data = new ResourcePackRootEntry();
                    Body.Data.PayloadLength = br.BaseStream.Length - origin;

                    Body.Data.Read(br, origin);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading RPK file: " + e.Message);
            }
        }

        public void Write(string path)
        {
            try
            {
                using FileStream fs = File.Create(path);
                using BinaryWriter bw = new BinaryWriter(fs);
                {
                    // --- Layout pass ---
                    long dataSize = Body.Data.GetPayloadSize();
                    Body.Data.PayloadLength = dataSize;

                    Header.TotalDataSize = (uint)dataSize;

                    // --- Write header ---
                    Header.Write(bw);

                    // --- Write body ---
                    Body.Data.RelativeOffset = 0;
                    Body.Data.Write(bw, 176);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing RPK file: " + e.Message);
            }
        }

        public void WriteAllAssetsToFile(string baseDir)
        {
            try
            {
                foreach (var entry in Body.Data.Table.Entries)
                {
                    if (entry is AssetListContainer)
                    {
                        AssetList? list = ((AssetListContainer)entry).Table.Entries.OfType<AssetList>().FirstOrDefault();
                        Directory.CreateDirectory(baseDir);
                        list.WriteToFiles(baseDir);
                    }
                    else if (entry is XMLEntry)
                    {
                        Directory.CreateDirectory(baseDir);
                        ((XMLEntry)entry).WriteToFile(baseDir);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing RPK assets to thier respective file format: " + e.Message);
            }
        }
    }
}
