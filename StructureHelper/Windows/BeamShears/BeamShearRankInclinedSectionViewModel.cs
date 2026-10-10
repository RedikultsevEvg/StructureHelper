using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelper.Windows.UserControls.WorkPlanes;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearRankInclinedSectionViewModel : ViewModelBase
    {
        private IBeamShearActionResult actionResult;

        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearActionResult> logic;

        public WorkPlaneRootViewModel WorkPlaneRoot { get; private set; }

        public BeamShearRankInclinedSectionViewModel(IBeamShearActionResult actionResult)
        {
            this.actionResult = actionResult;
            Process();
        }

        private void Process()
        {
            WorkPlaneRoot = new();
            logic = new ActionResultToGraphicalPrimitivesConvertLogic();
            var primitives = logic.Convert(actionResult);
            primitives.ForEach(primitive =>
            {
                WorkPlaneRoot.PrimitiveCollection.Primitives.Add(primitive);
            });
        }
    }
}
