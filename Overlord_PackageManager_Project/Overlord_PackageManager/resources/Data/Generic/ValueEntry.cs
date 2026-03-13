namespace Overlord_PackageManager.resources.Data.Generic
{
    public abstract class ValueEntry<T>(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public T Value { get; set; }
    }
}