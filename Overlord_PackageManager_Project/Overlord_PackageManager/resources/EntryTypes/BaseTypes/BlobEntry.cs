using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class BlobEntry(uint id, uint relOffset) : ValueEntry<byte[]>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadBytes((int)PayloadLength);
        }

        public override long GetPayloadSize()
        {
            if (Value != null)
            {
                return Value.Length;
            }
            else
            {
                return 0;
            }
        }


        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            writer.Write(Value);
        }
    }
}
