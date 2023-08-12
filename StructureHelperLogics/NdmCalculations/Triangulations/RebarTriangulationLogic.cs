using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Ndms.Transformations;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    internal class RebarTriangulationLogic : ITriangulationLogic
    {
        private readonly RebarTriangulationLogicOptions options;
        public RebarTriangulationLogic(ITriangulationLogicOptions options)
        {
            ValidateOptions(options);
            this.options = options as RebarTriangulationLogicOptions;
        }
        public IEnumerable<INdm> GetNdmCollection()
        {
            var concreteNdm = new Ndm
            {
                CenterX = options.Center.X,
                CenterY = options.Center.Y,
                Area = options.Area,
                Material = options.HostMaterial.GetLoaderMaterial(options.triangulationOptions.LimiteState, options.triangulationOptions.CalcTerm),
                StressScale = -1d
            };
            var rebarNdm = new RebarNdm
            {
                CenterX = options.Center.X,
                CenterY = options.Center.Y,
                Area = options.Area,
                Material = options.HeadMaterial.GetLoaderMaterial(options.triangulationOptions.LimiteState, options.triangulationOptions.CalcTerm)
            };
            List<INdm> ndmCollection = new() { concreteNdm, rebarNdm};
            //List<INdm> ndmCollection = new() { rebarNdm };
            NdmTransform.SetPrestrain(ndmCollection, StrainTupleService.ConvertToLoaderStrainMatrix(options.Prestrain));
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            if (options is not RebarTriangulationLogicOptions)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(RebarTriangulationLogicOptions), options.GetType()));
            }
        }
    }
}
