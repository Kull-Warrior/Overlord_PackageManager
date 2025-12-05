using Overlord_PackageManager.resources.EntryTypes;
using Overlord_PackageManager.resources.EntryTypes.XML;
using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.RPK
{
    public class RpkHeader
    {
        public string Magic = "";          // should be "RPK\0"
        public uint Version;
        public uint FileId;
        public uint TotalDataSize;
        public string Name = "";

        public RpkHeader(BinaryReader br)
        {
            Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            Version = br.ReadUInt32();
            FileId = br.ReadUInt32();
            TotalDataSize = br.ReadUInt32();
            byte[] nameBytes = br.ReadBytes(160);
            Name = Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
        }
    }
    public class RPKBody
    {
        public RefTable Data;
    }

    public class RpkFile
    {
        public RpkHeader Header;
        public RPKBody Body;
        public void Parse(string path)
        {
            try
            {
                using FileStream fs = File.OpenRead(path);
                using BinaryReader br = new BinaryReader(fs);
                {
                    Header = new RpkHeader(br);
                    Body = new RPKBody();

                    Body.Data = new RefTable(br, Entry.RPKRootDictionary);

                    foreach (var entry in Body.Data.Entries)
                    {
                        if (entry is AssetList)
                        {
                            ((AssetList)entry).Read(br, Body.Data.origin);
                        }
                        if (entry is XMLEntry)
                        {
                            ((XMLEntry)entry).Read(br, Body.Data.origin, 0, Entry.XMLDictionary);
                        }
                        else if (entry is not RefTableEntry)
                        {
                            entry.Read(br, Body.Data.origin);
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                //throw;
            }
        }
    }
}
