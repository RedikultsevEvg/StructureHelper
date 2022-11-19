using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ShapeServices;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class LinePrimitive : INdmPrimitive, ILineShape, IHasDivisionSize, ISaveable, ICloneable
    {
        public ICenter Center { get; set; }
        public IShape Shape { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }

        public ICenter StartPoint { get; set; }
        public ICenter EndPoint { get; set; }
        public double Thickness { get; set; }

        public LinePrimitive()
        {
            StartPoint = new Center();
            EndPoint = new Center();

            Name = "New Line";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
        }

        public object Clone()
        {
            LinePrimitive primitive = new LinePrimitive();
            NdmPrimitivesService.CopyNdmProperties(this, primitive);
            NdmPrimitivesService.CopyDivisionProperties(this, primitive);
            ShapeService.CopyLineProperties(this, primitive);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
