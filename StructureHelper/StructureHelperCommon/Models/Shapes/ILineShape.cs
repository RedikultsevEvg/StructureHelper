﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public interface ILineShape : IShape
    {
        IPoint2D StartPoint { get; set; }
        IPoint2D EndPoint { get; set; }
        double Thickness { get; set; }
    }
}
