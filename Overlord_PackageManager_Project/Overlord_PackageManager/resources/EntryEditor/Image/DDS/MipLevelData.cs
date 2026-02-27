using Overlord_PackageManager.resources.EntryTypes.Image.DDS;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public class MipLevelData
    {
        public uint Width { get; }
        public uint Height { get; }
        public DDSFormat Format { get; }
        public byte[] Data { get; }

        public MipLevelData(uint width, uint height, DDSFormat format, byte[] data)
        {
            Width = width;
            Height = height;
            Format = format;
            Data = data;
        }
    }
}