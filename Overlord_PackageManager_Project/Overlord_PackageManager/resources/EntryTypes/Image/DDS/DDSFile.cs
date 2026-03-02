namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public sealed class DDSFile
    {
        public uint Width { get; init; }
        public uint Height { get; init; }
        public uint MipCount { get; init; }
        public DDSFormat Format { get; init; }
        public bool IsCubemap { get; init; }

        public IReadOnlyList<DDSMipFace> Faces { get; init; }
    }

    public sealed class DDSMipFace
    {
        public int FaceIndex { get; init; }
        public int MipIndex { get; init; }
        public uint Width { get; init; }
        public uint Height { get; init; }
        public byte[] Data { get; init; }
    }
}