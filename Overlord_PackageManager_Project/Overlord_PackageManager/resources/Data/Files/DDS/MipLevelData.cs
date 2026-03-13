namespace Overlord_PackageManager.resources.Data.Files.DDS
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