using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class PointTriangulationLogic : ITriangulationLogic
    {
        private readonly PointTriangulationLogicOptions options;

        public PointTriangulationLogic(ITriangulationLogicOptions options)
        {
            ValidateOptions(options);
            this.options = options as  PointTriangulationLogicOptions;
        }

        public IEnumerable<INdm> GetNdmCollection()
        {
            var ndm = new Ndm
            {
                CenterX = options.Center.X,
                CenterY = options.Center.Y,
                Area = options.Area,
                Material = options.HeadMaterial.GetLoaderMaterial(options.TriangulationOptions.LimiteState, options.TriangulationOptions.CalcTerm)
            };
            List<INdm> ndmCollection = new () { ndm};
            NdmTransform.SetPrestrain(ndmCollection, ForceTupleConverter.ConvertToLoaderStrainMatrix(options.Prestrain));
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            if (options is not PointTriangulationLogicOptions)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(PointTriangulationLogicOptions), options.GetType()));
            }
        }
    }
}
