using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;
using System.Collections.Generic;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.Models.Primitives
{
    public class PointPrimitive : IPointPrimitive
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }
        public double Area { get; set; }

        public IVisualProperty VisualProperty { get; }

        public PointPrimitive()
        {
            Name = "New Point";
            Area = 0.0005d;
            VisualProperty = new VisualProperty();
        }

        public PointPrimitive(IHeadMaterial material) : this() { HeadMaterial = material; }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            var options = new PointTriangulationLogicOptions(this);
            IPointTriangulationLogic logic = new PointTriangulationLogic(options);
            return logic.GetNdmCollection(material);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        { 
            var primitive = new PointPrimitive();
            NdmPrimitivesService.CopyNdmProperties(this, primitive);
            primitive.Area = Area;
            return primitive;
        }
    }
}
