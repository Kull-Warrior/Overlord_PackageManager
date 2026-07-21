using System.Numerics;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public sealed record Transform(
        Matrix4x4 Matrix,
        Quaternion Rotation,
        Vector4 Translation
    );
}