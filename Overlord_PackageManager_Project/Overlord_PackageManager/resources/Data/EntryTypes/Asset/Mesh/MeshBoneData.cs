using Overlord_PackageManager.resources.Data.Factories;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh
{
    public class MeshBoneData(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => MeshFactory.CreateMeshBoneData;
    }
}
