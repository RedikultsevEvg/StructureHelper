using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public interface IGetSectionEffectivenessLogic : ILogic
    {
        ISectionEffectiveness GetSectionEffectiveness(ShearCodeTypes shearCodeTypes, IShape shape);
    }
}
