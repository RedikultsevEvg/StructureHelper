using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    internal class CircleTriangulationLogic : ITriangulationLogic
    {
        CircleTriangulationLogicOptions options;
        public ITriangulationLogicOptions Options { get; private set; }
        public CircleTriangulationLogic(ITriangulationLogicOptions options)
        {
            ValidateOptions(options);
            this.options = options as CircleTriangulationLogicOptions;
            Options = options;
        }

        public IEnumerable<INdm> GetNdmCollection(IMaterial material)
        {

            double diameter = options.Circle.Diameter;
            double ndmMaxSize = options.NdmMaxSize;
            int ndmMinDivision = options.NdmMinDivision;
            var logicOptions = new LoaderCalculator.Triangulations.CircleTriangulationLogicOptions(diameter, ndmMaxSize, ndmMinDivision);
            var logic = LoaderCalculator.Triangulations.Triangulation.GetLogicInstance(logicOptions);
            var ndmCollection = logic.GetNdmCollection(new LoaderCalculator.Data.Planes.CirclePlane { Material = material });
            TriangulationService.CommonTransform(ndmCollection, options);
            TriangulationService.SetPrestrain(ndmCollection, options.Prestrain);
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            if (options is not ICircleTriangulationLogicOptions )
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"\n Expected: {nameof(ICircleTriangulationLogicOptions)}, But was: {nameof(options)}");
            }
        }
    }
}
