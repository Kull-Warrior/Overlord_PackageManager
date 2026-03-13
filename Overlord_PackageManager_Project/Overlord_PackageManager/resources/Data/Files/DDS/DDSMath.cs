namespace Overlord_PackageManager.resources.Data.Files.DDS
{
    public static class DDSMath
    {
        public static uint CalculateMipCount(uint width, uint height)
        {
            uint maxDim = Math.Max(width, height);
            return (uint)Math.Floor(Math.Log2(maxDim)) + 1;
        }

        public static uint CalculateMipByteSize(uint width, uint height, DDSFormat format)
        {
            if (format == DDSFormat.UncompressedRGB)
                return width * height * 3;

            if (format == DDSFormat.UncompressedRGBA)
                return width * height * 4;

            uint blockSize = format == DDSFormat.DXT1 ? 8u : 16u;
            uint blocksWide = (width + 3) / 4;
            uint blocksHigh = (height + 3) / 4;

            return blocksWide * blocksHigh * blockSize;
        }
    }
}