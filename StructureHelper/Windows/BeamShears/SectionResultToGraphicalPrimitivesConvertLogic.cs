using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;

namespace StructureHelper.Windows.BeamShears
{
    public class SectionResultToGraphicalPrimitivesConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearSectionLogicResult>
    {
        public List<IGraphicalPrimitive> Convert(IBeamShearSectionLogicResult source)
        {
            List<IGraphicalPrimitive> graphicalPrimitives = new List<IGraphicalPrimitive>();
            BeamShearSectionPrimitive beamShearSectionPrimitive = new(source.InputData.InclinedSection.BeamShearSection, source.InputData.InclinedSection);
            graphicalPrimitives.Add(beamShearSectionPrimitive);
            InclinedSectionPrimitive inclinedSectionPrimitive = new(source);
            graphicalPrimitives.Add((inclinedSectionPrimitive));
            return graphicalPrimitives;
        }
    }
}
