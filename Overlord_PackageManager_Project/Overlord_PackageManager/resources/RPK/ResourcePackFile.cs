using Overlord_PackageManager.resources.EntryTypes;
using Overlord_PackageManager.resources.EntryTypes.XML;
using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.RPK
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
    }
    public class ResourcePackBody
    {
        public ReferenceTable Data;
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

                    Body.Data = new ReferenceTable(br, Entry.ResourcePackRootTableDictionary);

                    foreach (var entry in Body.Data.Entries)
                    {
                        if (entry is AssetList)
                        {
                            ((AssetList)entry).Read(br, Body.Data.OffsetOrigin);
                        }
                        else if (entry is XMLEntry)
                        {
                            ((XMLEntry)entry).Read(br, Body.Data.OffsetOrigin, 0, Entry.XMLDictionary);
                        }
                        else if (entry is not ReferenceTableEntry)
                        {
                            entry.Read(br, Body.Data.OffsetOrigin);
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading RPK file: " + e.Message);
            }
        }
        public void WriteAllAssetsToFile(string baseDir)
        {
            try
            {
                foreach (var entry in Body.Data.Entries)
                {
                    if (entry is AssetList)
                    {
                        Directory.CreateDirectory(baseDir);
                        ((AssetList)entry).WriteToFiles(baseDir);
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
