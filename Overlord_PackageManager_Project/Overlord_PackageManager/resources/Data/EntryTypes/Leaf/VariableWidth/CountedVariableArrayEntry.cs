using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

public abstract class CountedVariableArrayEntry<T>(uint id, uint relOffset) : ValueEntry<T[]>(id, relOffset)
{
    protected abstract T ReadValue(BinaryReader reader);
    protected abstract void WriteValue(BinaryWriter writer, T value);
    protected abstract long GetValuePayloadSize(T value);

    public int Count => Value?.Length ?? 0;

    public override void Read(BinaryReader reader, long origin)
    {
        reader.BaseStream.Position = origin + RelativeOffset;

        int count = checked((int)reader.ReadUInt32());
        Value = new T[count];

        for (int i = 0; i < count; i++)
        {
            Value[i] = ReadValue(reader);
        }
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

    public override void Write(BinaryWriter writer, long origin)
    {
        writer.BaseStream.Position = origin + RelativeOffset;

        T[] values = Value ?? [];
        writer.Write((uint)values.Length);

        foreach (T value in values)
        {
            WriteValue(writer, value);
        }
    }
}