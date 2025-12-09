using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneScaleData()
    {
        public Half scale_x;
        public Half scale_y;
        public Half scale_z;

        public void Read(BinaryReader reader)
        {
            scale_x = reader.ReadHalf();
            scale_y = reader.ReadHalf();
            scale_z = reader.ReadHalf();
        }
    }
}
