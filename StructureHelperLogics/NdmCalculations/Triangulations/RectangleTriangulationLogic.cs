using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using LoaderCalculator.Data.Ndms.Transformations;
using LoaderCalculator.Data.Matrix;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class RectangleTriangulationLogic : IRectangleTriangulationLogic
    {
        IRectangleTriangulationLogicOptions options;
        public ITriangulationLogicOptions Options { get; }

        public IEnumerable<INdm> GetNdmCollection(IMaterial material)
        {
            double width = options.Rectangle.Width;
            double height = options.Rectangle.Height;
            double ndmMaxSize = options.NdmMaxSize;
            int ndmMinDivision = options.NdmMinDivision;
            LoaderCalculator.Triangulations.RectangleTriangulationLogicOptions logicOptions = new LoaderCalculator.Triangulations.RectangleTriangulationLogicOptions(width, height, ndmMaxSize, ndmMinDivision);
            var logic = LoaderCalculator.Triangulations.Triangulation.GetLogicInstance(logicOptions);
            var ndmCollection = logic.GetNdmCollection(new LoaderCalculator.Data.Planes.RectangularPlane { Material = material });
            TriangulationService.CommonTransform(ndmCollection, options);
            double angle = options.Rectangle.Angle;
            NdmTransform.Rotate(ndmCollection, angle);
            TriangulationService.SetPrestrain(ndmCollection, options.Prestrain);
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            if (options is not IRectangleTriangulationLogicOptions)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"\n Expected: {nameof(IRectangleTriangulationLogicOptions)}, But was: {nameof(options)}");
            }
        }

        public RectangleTriangulationLogic(ITriangulationLogicOptions options)
        {
            ValidateOptions(options);
            this.options = options as IRectangleTriangulationLogicOptions;
            Options = options;
        }
    }
}
