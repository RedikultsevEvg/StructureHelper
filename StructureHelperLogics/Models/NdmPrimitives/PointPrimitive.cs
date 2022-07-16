using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using StructureHelperLogics.NdmCalculations.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.NdmPrimitives
{
    public class PointPrimitive : IPrimitive
    {
        ICenter _center;
        IShape _shape;

        public ICenter Center => _center;
        public IShape Shape => _shape;
        public double Area
        {
            get
            {
                IPoint point = _shape as IPoint;
                return point.Area;
            }
            set
            {
                IPoint point = _shape as IPoint;
                point.Area = value;
            }
        }

        public PointPrimitive(ICenter center, IShape shape)
        {
            _center = center;
            _shape = shape;
        }
        public INdmPrimitive GetNdmPrimitive()
        {
            double strength = 400;
            string materialName = "s400";
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial() { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive() { Center = _center, Shape = _shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }

        private MaterialTypes GetMaterialTypes()
        {
            return MaterialTypes.Reinforcement;
        }
    }
}
