using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;

namespace StructureHelper.Windows.BeamShears
{
    public class SectionResultToGraphicalPrimitivesConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearSectionLogicResult>
    {
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IStirrup> stirrupLogic;
        private IInclinedSection inclinedSection;

        public List<IGraphicalPrimitive> Convert(IBeamShearSectionLogicResult source)
        {
            inclinedSection = source.InputData.InclinedSection;
            InitializeStrategies();
            List<IGraphicalPrimitive> graphicalPrimitives = new List<IGraphicalPrimitive>();
            BeamShearSectionPrimitive beamShearSectionPrimitive = new(source.ResultInputData.InclinedSection.BeamShearSection, inclinedSection);
            graphicalPrimitives.Add(beamShearSectionPrimitive);
            var supportInternalAction = source.ResultInputData.BeamShearAction.SupportAction.SupportForce.ForceTuple;
            ConcentratedForce SupportForce = new(Guid.Empty);
            SupportForce.ForceValue.Qy = supportInternalAction.Qy;
            ConcentratedForcePrimitive concentratedForcePrimitive = new(SupportForce);
            graphicalPrimitives.Add((concentratedForcePrimitive));
            graphicalPrimitives.AddRange(stirrupLogic.Convert(source.ResultInputData.Stirrup));
            InclinedSectionPrimitive inclinedSectionPrimitive = new(source);
            graphicalPrimitives.Add((inclinedSectionPrimitive));
            return graphicalPrimitives;
        }

        private void InitializeStrategies()
        {
            stirrupLogic ??= new StirrupToGraphicPrimitiveConvertLogic(inclinedSection);
        }
    }
}
