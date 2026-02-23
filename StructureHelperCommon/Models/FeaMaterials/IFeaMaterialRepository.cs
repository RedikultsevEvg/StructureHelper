using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IFeaMaterialRepository : ISaveable
    {
        List<IFeaMaterial> FeaMaterials { get; }
    }
}
