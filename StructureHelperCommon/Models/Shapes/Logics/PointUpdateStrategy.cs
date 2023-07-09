﻿using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes.Logics
{
    /// <inheritdoc />
    public class PointUpdateStrategy : IUpdateStrategy<IPoint2D>
    {
        /// <inheritdoc />
        public void Update(IPoint2D targetObject, IPoint2D sourceObject)
        {
            targetObject.X = sourceObject.X;
            targetObject.Y = sourceObject.Y;
        }
    }
}