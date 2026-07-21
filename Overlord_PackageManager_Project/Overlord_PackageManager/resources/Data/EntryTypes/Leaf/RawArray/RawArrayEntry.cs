using System.IO;
using Overlord_PackageManager.resources.Data.Generic;

public abstract class RawArrayEntry<T>(uint id, uint relOffset) : ValueEntry<T[]>(id, relOffset)
{
    protected abstract T ReadValue(BinaryReader reader);

    protected abstract void WriteValue(BinaryWriter writer, T value);

    protected abstract int ElementSize { get; }

    public override void Read(BinaryReader reader, long origin)
    {
        reader.BaseStream.Position = origin + RelativeOffset;

        int count = (int)(PayloadLength / ElementSize);

        Value = new T[count];

        for (int i = 0; i < count; i++)
        {
            Value[i] = ReadValue(reader);
        }
    }

    public override long GetPayloadSize()
    {
        return (Value?.Length ?? 0) * (long)ElementSize;
    }

    public override void Write(BinaryWriter writer, long origin)
    {
        writer.BaseStream.Position = origin + RelativeOffset;

        foreach (var value in Value)
        {
            WriteValue(writer, value);
        }
    }
}