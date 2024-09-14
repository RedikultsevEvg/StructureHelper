using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Ndms.Transformations;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Primitives;
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
            List<INdm> ndmCollection = new();
            if (options.HostPrimitive is not null)
            {
                var concreteNdm = GetConcreteNdm();
                ndmCollection.Add(concreteNdm);
            }

            var rebarNdm = GetRebarNdm();
            ndmCollection.Add(rebarNdm);
            return ndmCollection;
        }

        public RebarNdm GetRebarNdm()
        {
            var rebarNdm = new RebarNdm
            {
                CenterX = options.Center.X,
                CenterY = options.Center.Y,
                Area = options.Area,
                Material = options.HeadMaterial.GetLoaderMaterial(options.triangulationOptions.LimiteState, options.triangulationOptions.CalcTerm)
            };
            ;
            NdmTransform.SetPrestrain(rebarNdm, TupleConverter.ConvertToLoaderStrainMatrix(options.Prestrain));
            return rebarNdm;
        }

        public Ndm GetConcreteNdm()
        {
            var hostPrimitive = options.HostPrimitive;
            var material = hostPrimitive
                .NdmElement
                .HeadMaterial
                .GetLoaderMaterial(options.triangulationOptions.LimiteState, options.triangulationOptions.CalcTerm);
            
            var prestrain = ForceTupleService.SumTuples(hostPrimitive.NdmElement.UsersPrestrain,
                hostPrimitive.NdmElement.AutoPrestrain)
                as StrainTuple;

            var concreteNdm = new Ndm
            {
                CenterX = options.Center.X,
                CenterY = options.Center.Y,
                //Area = options.Area, //to do solve problem with additional concrete ndm
                Area = 0,//options.Area, //to do solve problem with additional concrete ndm
                Material = material,
                //StressScale = -1d
                StressScale = 1d//-1d
            };
            NdmTransform.SetPrestrain(concreteNdm, TupleConverter.ConvertToLoaderStrainMatrix(prestrain));
            return concreteNdm;
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
