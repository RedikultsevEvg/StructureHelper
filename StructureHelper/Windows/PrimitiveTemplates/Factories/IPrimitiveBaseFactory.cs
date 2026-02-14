using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;

namespace StructureHelper.Windows.PrimitiveTemplates.Factories
{
    public interface IPrimitiveBaseFactory
    {
        PrimitiveBase GetPrimitive(PrimitiveType primitiveType);
    }
}