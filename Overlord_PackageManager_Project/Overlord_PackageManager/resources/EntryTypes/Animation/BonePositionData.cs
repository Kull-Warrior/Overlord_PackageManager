using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BonePositionData()
    {
        public uint timestamp;
        public float x;
        public float y;
        public float z;

        public void Read(BinaryReader reader)
        {
            timestamp = reader.ReadUInt32();
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
        }
    }
}
