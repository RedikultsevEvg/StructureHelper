using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;

namespace StructureHelper.Windows.BeamShears
{
    public class SectionResultToGraphicalPrimitivesConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearSectionLogicResult>
    {
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearAction> actionLogic;
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IStirrup> stirrupLogic;
        private IInclinedSection inclinedSection;

        public List<IGraphicalPrimitive> Convert(IBeamShearSectionLogicResult source)
        {
            inclinedSection = source.InputData.InclinedSection;
            InitializeStrategies();
            List<IGraphicalPrimitive> graphicalPrimitives = new();
            BeamShearSectionPrimitive beamShearSectionPrimitive = new(source.ResultInputData.InclinedSection.BeamShearSection, inclinedSection);
            graphicalPrimitives.Add(beamShearSectionPrimitive);
            graphicalPrimitives.AddRange(actionLogic.Convert(source.ResultInputData.BeamShearAction));
            graphicalPrimitives.AddRange(stirrupLogic.Convert(source.ResultInputData.Stirrup));
            InclinedSectionPrimitive inclinedSectionPrimitive = new(source);
            graphicalPrimitives.Add(inclinedSectionPrimitive);
            return graphicalPrimitives;
        }



        private void InitializeStrategies()
        {
            stirrupLogic ??= new StirrupToGraphicPrimitiveConvertLogic(inclinedSection);
            actionLogic ??= new ActionToGraphicPrimitiveConvertLogic(inclinedSection);
        }
    }
}
