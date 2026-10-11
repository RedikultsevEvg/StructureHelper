using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelper.Windows.UserControls.WorkPlanes;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;

namespace StructureHelper.Windows.BeamShears
{
    public class InclinedSectionViewerViewModel : ViewModelBase
    {
        private IBeamShearSectionLogicResult sectionResult;
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearSectionLogicResult> sectionLogic;

        public WorkPlaneRootViewModel WorkPlaneRoot { get; private set; }

        public InclinedSectionViewerViewModel(IBeamShearSectionLogicResult sectionResult)
        {
            this.sectionResult = sectionResult;
            WorkPlaneRoot = new();
            sectionLogic = new SectionResultToGraphicalPrimitivesConvertLogic();
            var sectionPrimitives = sectionLogic.Convert(this.sectionResult);
            sectionPrimitives.ForEach(primitive =>
            {
                WorkPlaneRoot.PrimitiveCollection.Primitives.Add(primitive);
            });
        }
    }
}
