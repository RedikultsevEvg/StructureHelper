using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class Point3D : IPoint3D
    {
        private readonly IUpdateStrategy<IPoint3D> updateStrategy = new Point3DUpdateStrategy();

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double X { get; set; }
        /// <inheritdoc/>
        public double Y { get; set; }
        /// <inheritdoc/>
        public double Z { get; set; }

        public Point3D(Guid id)
        {
            Id = id;
        }

        public Point3D() : this(Guid.NewGuid()) { }

        public object Clone()
        {
            var newItem = new Point3D();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
