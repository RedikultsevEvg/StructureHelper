using StructureHelperCommon.Infrastructures.Interfaces;
using System.Reflection;

namespace DataAccess.JsonConverters
{
    /// <summary>
    /// Helper logic to check if a property should be ignored
    /// </summary>
    public interface IShouldIgnorePropertyLogic : ILogic
    {
        bool ShouldIgnoreProperty(PropertyInfo prop);
    }
}