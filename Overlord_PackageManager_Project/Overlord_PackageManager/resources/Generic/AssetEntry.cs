namespace Overlord_PackageManager.resources.Generic
{
    public class AssetEntry(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        public uint TypeIdentifier { get; set; }
    }
}
