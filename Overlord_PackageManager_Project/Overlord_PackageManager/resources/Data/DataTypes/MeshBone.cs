using System.Numerics;

namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public sealed record MeshBone(
        char[] Name,
        MeshTransform Transform,
        int Unknown1,
        int Unknown2,
        int Unknown3,
        int Unknown4,
        int Unknown5,
        int Unknown6
    );
}