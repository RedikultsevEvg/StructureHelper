﻿using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class LinePrimitive : ILinePrimitive
    {
        public Guid Id { get;}
        public string Name { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }

        public IPoint2D StartPoint { get; set; }
        public IPoint2D EndPoint { get; set; }
        public double Thickness { get; set; }

        public IVisualProperty VisualProperty => throw new NotImplementedException();

        public StrainTuple UsersPrestrain => throw new NotImplementedException();

        public StrainTuple AutoPrestrain => throw new NotImplementedException();

        public bool ClearUnderlying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Triangulate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LinePrimitive()
        {
            StartPoint = new Point2D();
            EndPoint = new Point2D();

            Name = "New Line";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
        }

        public object Clone()
        {
            var primitive = new LinePrimitive();
            throw new NotImplementedException();

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

        public bool IsPointInside(IPoint2D point)
        {
            throw new NotImplementedException();
        }
    }
}
