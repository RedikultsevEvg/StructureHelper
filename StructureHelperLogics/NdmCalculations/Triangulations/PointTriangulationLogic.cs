using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class PointTriangulationLogic : IPointTriangulationLogic
    {
        public ITriangulationLogicOptions Options { get; }

        public PointTriangulationLogic(ITriangulationLogicOptions options)
        {
            Options = options;
        }

        public IEnumerable<INdm> GetNdmCollection(IMaterial material)
        {
            IPointTriangulationLogicOptions options = Options as IPointTriangulationLogicOptions;
            ICenter center = options.Center;
            double area = options.Area;
            List<INdm> ndmCollection = new List<INdm>();
            INdm ndm = new Ndm { CenterX = center.X, CenterY = center.Y, Area = area, Material = material };
            ndmCollection.Add(ndm);
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
