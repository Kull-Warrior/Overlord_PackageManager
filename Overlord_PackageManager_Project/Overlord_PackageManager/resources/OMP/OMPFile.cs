using Overlord_PackageManager.resources.EntryTypes;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Lua;
using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.OMP
{
    public class OMPHeader
    {
        public string Magic;
        public byte[] Unknown;

        public OMPHeader(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            Magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            Unknown = reader.ReadBytes(39);
        }
    }
    public class OMPBody
    {
        public ReferenceTable Info;
        public ReferenceTable Data;
    }

    public class OMPFile
    {
        public OMPHeader Header;
        public OMPBody Body;

        public void Parse(string path)
        {
            try
            {
                using FileStream fs = File.OpenRead(path);
                using BinaryReader br = new BinaryReader(fs);
                {
                    Header = new OMPHeader(br);
                    Body = new OMPBody();

                    Body.Info = new ReferenceTable(br, br.BaseStream.Length, Entry.InfoTableDictionary);

                    foreach (var entry in Body.Info.Entries)
                    {
                        entry.Read(br, Body.Info.PayloadStartOffset);
                    }

                    Body.Data = new ReferenceTable(br, br.BaseStream.Length,Entry.OMPDataRootTableDictionary);

                    foreach (var entry in Body.Data.Entries)
                    {
                        if (entry is TerrainDataEntry)
                        {
                            ((TerrainDataEntry)entry).Read(br, Body.Data.PayloadStartOffset, Entry.TerrainDataDictionary);
                        }

                        if (entry is UnknownTableType21Entry)
                        {
                            ((UnknownTableType21Entry)entry).Read(br, Body.Data.PayloadStartOffset, Entry.RPKListDictionary);
                        }

                        if (entry is ResourcePackLinkEntry)
                        {
                            ((ResourcePackLinkEntry)entry).Read(br, Body.Data.PayloadStartOffset, Entry.RPKListDictionary);
                        }

                        if (entry is LuaListEntry)
                        {
                            ((LuaListEntry)entry).Read(br, Body.Data.PayloadStartOffset, Entry.LuaListDictionary);
                        }

                        if (entry is LuaEntry)
                        {
                            ((LuaEntry)entry).Read(br, Body.Data.PayloadStartOffset, Entry.LuaDataDictionary);
                        }
                        
                        if (entry is StringEntry || entry is Int32Entry || entry is FloatEntry || entry is Int64Entry || entry is BlobEntry)
                        {
                            entry.Read(br, Body.Data.PayloadStartOffset);
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
