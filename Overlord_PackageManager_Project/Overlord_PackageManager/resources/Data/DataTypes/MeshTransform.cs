using System.Numerics;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public sealed record MeshTransform(
        Matrix4x4 Matrix,
        Vector3 Scale,
        Vector3 Translation,
        Quaternion Rotation
    );
}