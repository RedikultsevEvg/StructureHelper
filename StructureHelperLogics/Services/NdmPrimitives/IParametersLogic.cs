using StructureHelperCommon.Models.Parameters;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public interface IParametersLogic
    {
        List<IValueParameter<string>> GetTextParameters();
    }
}