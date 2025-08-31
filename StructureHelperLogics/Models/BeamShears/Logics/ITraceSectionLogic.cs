

using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface ITraceSectionLogic : ILogic
    {
        void TraceSection(IBeamShearSection section);
    }
}