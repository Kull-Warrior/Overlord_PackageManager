namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public sealed record ObjectBone(
        char[] Name,
        Transform Transform,
        int SkinID,
        int ParentIndex,
        int NextSiblingIndex,
        int FirstChildIndex,
        int Reserved
    );
}