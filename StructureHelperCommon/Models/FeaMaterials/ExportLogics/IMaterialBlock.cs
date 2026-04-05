using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IMaterialBlock :
        IAbaqusScriptBlock,
        IRequires<IModelContext>,
        IProvides<IMaterialContext>
    {
        string ModelVariableName { get; set; }
        string MaterialVariableName { get; set; }
    }
}
