using System.Numerics;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public record MeshBoneShape(Matrix3x3 Matrix, Vector3 Head, Vector3 Tail);
}