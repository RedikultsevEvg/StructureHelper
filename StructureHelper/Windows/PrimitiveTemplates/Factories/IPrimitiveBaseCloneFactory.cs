using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Windows.PrimitiveTemplates.Factories
{
    public interface IPrimitiveBaseCloneFactory
    {
        PrimitiveBase GetCloneByNdmPrimitive(INdmPrimitive ndmPrimitive);
    }
}