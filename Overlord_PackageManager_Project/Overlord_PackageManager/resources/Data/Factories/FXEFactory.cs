using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    internal class FXEFactory
    {
        public static Entry CreateFXE(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                //20 => new Int32Entry(id, relOffset),    // Length of FXE Data
                //21 => new BlobEntry(id, relOffset),    // FXE Data, Header + Raw Data, full FXE file
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}