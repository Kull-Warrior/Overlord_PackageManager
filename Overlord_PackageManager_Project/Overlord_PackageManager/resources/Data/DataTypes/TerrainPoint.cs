namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public sealed record TerrainPoint(
        float Height,
        byte MainTextureIndex,
        byte HasFoliage,
        byte CliffTextureIndex,
        byte UnknownIndex
    );
}