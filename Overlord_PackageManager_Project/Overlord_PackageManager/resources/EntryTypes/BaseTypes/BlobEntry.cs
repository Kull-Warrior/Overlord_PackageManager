using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    class BlobEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] varBytes;

        public void Read(BinaryReader reader, long origin, uint length)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varBytes = reader.ReadBytes((int)length);
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
