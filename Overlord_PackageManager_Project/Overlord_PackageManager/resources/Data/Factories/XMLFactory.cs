using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class XMLFactory
    {
        public static Entry CreateXML(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                10 => new StringEntry(id, relOffset),
                11 => new UInt32Entry(id, relOffset),
                12 => new ByteArrayEntry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}