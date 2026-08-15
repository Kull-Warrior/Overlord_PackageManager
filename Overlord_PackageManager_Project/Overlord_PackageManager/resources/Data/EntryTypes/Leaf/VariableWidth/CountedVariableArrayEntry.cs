using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

public abstract class CountedVariableArrayEntry<T>(uint id, uint relOffset) : ValueEntry<T[]>(id, relOffset)
{
    protected abstract T ReadElement(BinaryReader reader);
    protected abstract void WriteElement(BinaryWriter writer, T value);
    protected abstract long GetValuePayloadSize(T value);

    public int Count => Value?.Length ?? 0;

    protected override T[] ReadValue(BinaryReader reader)
    {
        int count = checked((int)reader.ReadUInt32());

        T[] values = new T[count];

        for (int i = 0; i < count; i++)
        {
            values[i] = ReadElement(reader);
        }

        return values;
    }

    public override long GetPayloadSize()
    {
        long size = sizeof(uint);

        foreach (T value in Value ?? [])
        {
            size += GetValuePayloadSize(value);
        }

        return size;
    }

    protected override void WriteValue(BinaryWriter writer, T[] value)
    {
        T[] values = value ?? [];

        writer.Write((uint)values.Length);

        foreach (T item in values)
        {
            WriteElement(writer, item);
        }
    }
}