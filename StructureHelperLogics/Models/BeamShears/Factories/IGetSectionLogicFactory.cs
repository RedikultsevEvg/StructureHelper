using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetSectionLogicFactory : ILogic
    {
        IBeamShearSectionLogic GetSectionLogic(ShearCodeTypes shearCodeTypes);
    }
}