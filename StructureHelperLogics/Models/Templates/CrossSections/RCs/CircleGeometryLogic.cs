﻿using StructureHelper.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public class CircleGeometryLogic : IRCGeometryLogic
    {
        ICircleTemplate template;
        CirclePrimitive concreteBlock;
        
        public IEnumerable<IHeadMaterial> HeadMaterials { get; set; }

        public CircleGeometryLogic(ICircleTemplate template)
        {
            this.template = template;
        }

        public IEnumerable<INdmPrimitive> GetNdmPrimitives()
        {
            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            primitives.AddRange(GetConcretePrimitives());
            primitives.AddRange(GetReinfrocementPrimitives());
            return primitives;
        }

        private IEnumerable<INdmPrimitive> GetConcretePrimitives()
        {
            var diameter = template.Shape.Diameter;
            var concreteMaterial = HeadMaterials.ToList()[0];
            var primitives = new List<INdmPrimitive>();
            concreteBlock = new CirclePrimitive() { Diameter = diameter, Name = "Concrete block", HeadMaterial = concreteMaterial };
            primitives.Add(concreteBlock);
            return primitives;
        }

        private IEnumerable<INdmPrimitive> GetReinfrocementPrimitives()
        {
            var reinforcementMaterial = HeadMaterials.ToList()[1];
            var radius = template.Shape.Diameter / 2 - template.CoverGap;
            var dAngle = 2d * Math.PI / template.BarCount;
            var barArea = Math.PI* template.BarDiameter* template.BarDiameter / 4d;
            var primitives = new List<INdmPrimitive>();
            for (int i = 0; i < template.BarCount; i++)
            {
                var angle = i * dAngle;
                var x = radius * Math.Sin(angle);
                var y = radius * Math.Cos(angle);
                var point = new RebarPrimitive()
                { 
                    Area = barArea,
                    Name = "Left bottom point",
                    HeadMaterial = reinforcementMaterial,
                    HostPrimitive=concreteBlock };
                point.Center.X = x;
                point.Center.Y = y;
                primitives.Add(point);
            }
            return primitives;
        }
    }
}
