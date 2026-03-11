using Overlord_PackageManager.resources.EntryTypes;
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
        public MapInfoEntry Info;
        public MapRootEntry Data;
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

                    long origin = br.BaseStream.Position;
                    Body.Data = new MapRootEntry();
                    Body.Data.PayloadLength = br.BaseStream.Length - origin;
                    Body.Info.Read(br, origin);

                    origin = br.BaseStream.Position;
                    Body.Data = new MapRootEntry();
                    Body.Data.PayloadLength = br.BaseStream.Length - origin;
                    Body.Data.Read(br, origin);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading overlord map file: " + e.Message);
            }

        }
    }
}
