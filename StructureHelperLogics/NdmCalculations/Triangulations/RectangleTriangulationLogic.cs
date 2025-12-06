using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class RectangleTriangulationLogic : ITriangulationLogic
    {
        private readonly RectangleTriangulationLogicOptions options;
        public RectangleTriangulationLogic(ITriangulationLogicOptions options)
        {
            ValidateOptions(options);
            this.options = options as RectangleTriangulationLogicOptions;
        }
        public IEnumerable<INdm> GetNdmCollection()
        {
            double width = options.Rectangle.Width;
            double height = options.Rectangle.Height;
            double ndmMaxSize = options.DivisionSize.NdmMaxSize;
            int ndmMinDivision = options.DivisionSize.NdmMinDivision;
            LoaderCalculator.Triangulations.RectangleTriangulationLogicOptions logicOptions = new LoaderCalculator.Triangulations.RectangleTriangulationLogicOptions(width, height, ndmMaxSize, ndmMinDivision);
            var logic = LoaderCalculator.Triangulations.Triangulation.GetLogicInstance(logicOptions);
            var ndmCollection = logic.GetNdmCollection(new LoaderCalculator.Data.Planes.RectangularPlane
            {
                Material = options.HeadMaterial.GetLoaderMaterial(options.TriangulationOptions.LimiteState, options.TriangulationOptions.CalcTerm)
            });
            TriangulationService.CommonTransform(ndmCollection, options);
            double angle = options.RotationAngle;
            NdmTransform.Rotate(ndmCollection, angle);
            TriangulationService.SetPrestrain(ndmCollection, options.Prestrain);
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            if (options is not RectangleTriangulationLogicOptions)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(RectangleTriangulationLogicOptions), options.GetType()));
            }
        }

    }
}
