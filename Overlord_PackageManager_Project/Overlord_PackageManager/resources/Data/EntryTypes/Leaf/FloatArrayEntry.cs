using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public class FloatArrayEntry(uint id, uint relOffset) : ValueEntry<float[]>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new float[PayloadLength / 4];
            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = reader.ReadSingle();
            }
        }

        public override long GetPayloadSize()
        {
            return (long)Value.Length * sizeof(float);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            for (int i = 0; i < Value.Length; i++)
            {
                writer.Write(Value[i]);
            }
        }
    }
}
