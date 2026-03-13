using System.IO;

namespace Overlord_PackageManager.resources.Data.Generic
{
    public abstract class Entry
    {
        public uint Id;
        public uint RelativeOffset;
        // Absolute payload size of this entry
        public long PayloadLength;

        protected Entry()
        {

        }

        protected Entry(uint id, uint relOffset)
        {
            Id = id;
            RelativeOffset = relOffset;
        }

        // Each entry knows how to read itself
        public abstract void Read(BinaryReader reader, long origin);

        // Each entry knows how to compute its own length based on its content, this is used when writing the entry back to a file
        public abstract long GetPayloadSize();

        // Each entry knows how to write itself
        public abstract void Write(BinaryWriter writer, long origin);
    }
}