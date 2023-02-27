﻿using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.ShapeServices;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class CirclePrimitive : ICirclePrimitive
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public IHeadMaterial? HeadMaterial { get; set; }

        public IStrainTuple UsersPrestrain { get; }

        public IStrainTuple AutoPrestrain { get; }

        public IVisualProperty VisualProperty { get; }

        public double Diameter { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }

        public CirclePrimitive()
        {
            Name = "New Circle";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
            VisualProperty = new VisualProperty { Opacity = 0.8d };
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
        }

        public object Clone()
        {
            var primitive = new CirclePrimitive();
            NdmPrimitivesService.CopyDivisionProperties(this, primitive);
            ShapeService.CopyCircleProperties(this, primitive);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            var ndms = new List<INdm>();
            var options = new CircleTriangulationLogicOptions(this);
            var logic = new CircleTriangulationLogic(options);
            ndms.AddRange(logic.GetNdmCollection(material));
            return ndms;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}