namespace StructureHelperLogics.Models.BeamShears
{
    public interface ISectionEffectivenessFactory
    {
        ISectionEffectiveness GetShearEffectiveness(BeamShearSectionType sectionType);
    }
}