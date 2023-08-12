using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
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
        private readonly CircleTriangulationLogicOptions options;
        public CircleTriangulationLogic(ITriangulationLogicOptions options)
        {
            ValidateOptions(options);
            this.options = options as CircleTriangulationLogicOptions;
        }

        public IEnumerable<INdm> GetNdmCollection()
        {

            double diameter = options.Circle.Diameter;
            double ndmMaxSize = options.NdmMaxSize;
            int ndmMinDivision = options.NdmMinDivision;
            var logicOptions = new LoaderCalculator.Triangulations.CircleTriangulationLogicOptions(diameter, ndmMaxSize, ndmMinDivision);
            var logic = LoaderCalculator.Triangulations.Triangulation.GetLogicInstance(logicOptions);
            var ndmCollection = logic.GetNdmCollection(new LoaderCalculator.Data.Planes.CirclePlane
            {
                Material = options.HeadMaterial.GetLoaderMaterial(options.triangulationOptions.LimiteState, options.triangulationOptions.CalcTerm)
            });
            TriangulationService.CommonTransform(ndmCollection, options);
            TriangulationService.SetPrestrain(ndmCollection, options.Prestrain);
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            if (options is not CircleTriangulationLogicOptions )
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(CircleTriangulationLogicOptions), options.GetType()));
            }
        }
    }
}
