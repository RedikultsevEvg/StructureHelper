using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelperLogics.Data.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class PointTriangulationLogic : IPointTiangulationLogic
    {
        public ITriangulationLogicOptions Options { get; }

        public PointTriangulationLogic(IPointTriangulationLogicOptions options)
        {
            Options = options;
        }

        public IEnumerable<INdm> GetNdmCollection(IMaterial material)
        {
            IPointTriangulationLogicOptions options = Options as IPointTriangulationLogicOptions;
            ICenter center = options.Center;
            double area = options.Area;
            List<INdm> ndmCollection = new List<INdm>();
            INdm ndm = new Ndm() { CenterX = center.X, CenterY = center.Y, Area = area, Material = material };
            ndmCollection.Add(ndm);
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
