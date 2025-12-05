using System.IO;

namespace Overlord_PackageManager.resources.RPK
{
    /// <summary>
    /// File‐level footer (16 bytes)
    /// </summary>
    public class RpkFooter
    {
        public uint Magic1;      // 0xDEADBEEF
        public uint Magic2;      // 0xFEEDDEAF
        public uint Checksum;    // CRC32 of data section
        public uint Constant;    // always 0x10C

        public static RpkFooter Read(BinaryReader br)
        {
            var footer = new RpkFooter();
            footer.Magic1 = br.ReadUInt32();
            footer.Magic2 = br.ReadUInt32();
            footer.Checksum = br.ReadUInt32();
            footer.Constant = br.ReadUInt32();
            return footer;
        }
    }
}
