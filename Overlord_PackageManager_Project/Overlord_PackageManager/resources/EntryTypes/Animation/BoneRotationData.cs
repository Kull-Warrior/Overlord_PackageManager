using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneRotationData()
    {
        public uint pitch;
        public uint yaw;
        public uint roll;

        public void Read(BinaryReader reader)
        {
            pitch = reader.ReadUInt32();
            yaw = reader.ReadUInt32();
            roll = reader.ReadUInt32();
        }
    }
}
