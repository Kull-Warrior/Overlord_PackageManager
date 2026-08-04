using System.Numerics;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public record MeshCluster(Matrix3x3 Matrix, Vector3 Center, Vector3 Extents, ushort PatchIndex, ushort TriangleCount);
}