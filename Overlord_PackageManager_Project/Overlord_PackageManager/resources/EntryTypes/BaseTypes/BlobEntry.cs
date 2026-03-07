using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class BlobEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] varBytes;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            varBytes = reader.ReadBytes((int)PayloadLength);
        }
    }
}
