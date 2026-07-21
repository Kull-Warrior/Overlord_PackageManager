namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public sealed record ObjectBone(
        char[] Name,
        Transform Transform,
        int ParentIndex,
        int NextSiblingIndex,
        int NextTraversalIndex,
        int Reserved
    );
}