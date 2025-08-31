using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IActionResultListLogic : ILogic
    {
        List<IBeamShearActionResult> GetActionResults();
    }
}