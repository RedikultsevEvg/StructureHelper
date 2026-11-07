using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IFeaMaterialRepository : ISaveable, ICloneStrategy<IFeaMaterialRepository>
    {
        List<IFeaMaterial> FeaMaterials { get; }
    }
}
