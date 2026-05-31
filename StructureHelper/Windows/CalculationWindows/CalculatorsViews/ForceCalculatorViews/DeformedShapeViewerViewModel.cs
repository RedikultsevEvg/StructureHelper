using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class DeformedShapeViewerViewModel
    {
        private IEnumerable<INdmPrimitive> ndmPrimitives;

        public ContourViewportViewModel ViewportViewModel { get; } = new ContourViewportViewModel();

        public void Rebuild()
        {
            var logic = new GetModels3dByValuePrimivesLogic()
            {
                InvertNormal = false,
            };
            logic.GetModels3d(ndmPrimitives, ViewportViewModel.Viewport3D);
        }

        public DeformedShapeViewerViewModel(IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            this.ndmPrimitives = ndmPrimitives;

        }
    }
}
