using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace DataAccess.DTOs
{
    /// <summary>
    /// Logic for antities which have force actions
    /// </summary>
    public interface IHasForceActionsProcessLogic : IProcessLogic<IHasForceActions>
    {
    }
}