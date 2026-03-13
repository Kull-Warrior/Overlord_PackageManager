using System.IO;
using System.Text;
using Overlord_PackageManager.resources.Data.EntryTypes.Map;

namespace Overlord_PackageManager.resources.Data.Files.OMP
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

        public void Write(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Magic));

            byte[] unknownBytes = new byte[39];
            if (Unknown != null)
                Array.Copy(Unknown, unknownBytes, Math.Min(Unknown.Length, 39));

            bw.Write(unknownBytes);
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
                    Body.Info = new MapInfoEntry();
                    Body.Info.PayloadLength = br.BaseStream.Length - origin;
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

        public void Write(string path)
        {
            try
            {
                using FileStream fs = File.Create(path);
                using BinaryWriter bw = new BinaryWriter(fs);
                {
                    // ---- Layout pass ----
                    long infoSize = Body.Info.GetPayloadSize();
                    Body.Info.PayloadLength = infoSize;

                    long dataSize = Body.Data.GetPayloadSize();
                    Body.Data.PayloadLength = dataSize;

                    // ---- Write header ----
                    Header.Write(bw);

                    // ---- Write body ----
                    long origin = bw.BaseStream.Position;

                    Body.Info.RelativeOffset = 0;
                    Body.Info.Write(bw, origin);

                    origin = bw.BaseStream.Position;

                    Body.Data.RelativeOffset = 0;
                    Body.Data.Write(bw, origin);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing overlord map file: " + e.Message);
            }
        }
    }
}