using System.IO;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public class BinaryType<T>
    {
        public required int Size;

        public required string DisplayName;

        public required Func<BinaryReader, T> Read;

        public required Action<BinaryWriter, T> Write;
    }
}