﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class CircleShape : ICircleShape
    {
        public Guid Id { get; }
        public double Diameter { get; set; }
        public CircleShape(Guid id)
        {
            Id = id;
        }

        public CircleShape() : this (Guid.NewGuid())
        {
            
        }

    }
}
