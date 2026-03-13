using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                11 => new Int32Entry(id, relOffset),
                12 => new BlobEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}