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

                    Body.Info = new ReferenceTable(br, Entry.InfoTableDictionary);

                    foreach (var entry in Body.Info.Entries)
                    {
                        entry.Read(br, Body.Info.OffsetOrigin);
                    }

                    Body.Data = new ReferenceTable(br, Entry.OMPDataRootTableDictionary);

                    foreach (var entry in Body.Data.Entries)
                    {
                        if ( entry is ReferenceTableEntry)
                        {
                            switch (entry.Id)
                            {
                                case 21:
                                    ((ReferenceTableEntry)entry).Read(br, Body.Data.OffsetOrigin, 0, Entry.UnknownType21Dictionary);

                                    foreach (var subEntry in ((ReferenceTableEntry)entry).Table.Entries)
                                    {
                                        if (entry is ReferenceTableEntry)
                                        {

                                        }
                                        else
                                        {
                                            subEntry.Read(br, ((ReferenceTableEntry)entry).Table.OffsetOrigin);
                                        }
                                    }
                                    break;
                                case 26:
                                    break;
                                case 27:
                                    break;
                                case 28:
                                    break;
                                case 30:
                                    break;
                                case 48:
                                    break;
                                case 52:
                                    break;
                                case 53:
                                    break;
                                case 100:
                                    break;
                                case 101:
                                    break;
                                case 102:
                                    break;
                                case 103:
                                    break;
                                case 120:
                                    break;
                                case 124:
                                    break;
                                case 130:
                                    break;
                                case 131:
                                    break;
                                case 133:
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (entry is TerrainDataEntry)
                        {
                            ((TerrainDataEntry)entry).Read(br, Body.Data.OffsetOrigin, 0, Entry.TerrainDataDictionary);
                        }

                        if (entry is UnknownTableType21Entry)
                        {
                            ((UnknownTableType21Entry)entry).Read(br, Body.Data.OffsetOrigin, 0, Entry.RPKListDictionary);
                        }

                        if (entry is ResourcePackLinkEntry)
                        {
                            ((ResourcePackLinkEntry)entry).Read(br, Body.Data.OffsetOrigin, 3, Entry.RPKListDictionary);
                        }

                        if (entry is LuaListEntry)
                        {
                            ((LuaListEntry)entry).Read(br, Body.Data.OffsetOrigin, 3, Entry.LuaListDictionary);
                        }

                        if (entry is LuaEntry)
                        {
                            ((LuaEntry)entry).Read(br, Body.Data.OffsetOrigin, 0, Entry.LuaDataDictionary);
                        }
                        
                        if (entry is StringEntry || entry is Int32Entry || entry is FloatEntry || entry is Int64Entry)
                        {
                            entry.Read(br, Body.Data.OffsetOrigin);
                        }

                        if (entry is BlobEntry)
                        {
                            uint length;
                            switch (entry.Id)
                            {
                                case 22:
                                case 23:
                                case 24:
                                    length = 12;
                                    break;
                                case 31:
                                    length = 60;
                                    break;
                                case 132:
                                    length = 7;
                                    break;
                                default:
                                    length = 0;
                                    break;
                            }
                            ((BlobEntry)entry).Read(br, Body.Data.OffsetOrigin, length);
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
