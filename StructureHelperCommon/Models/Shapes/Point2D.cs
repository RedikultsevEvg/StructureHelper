using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes.Logics;
using System;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class Point2D : IPoint2D
    {
        private readonly IUpdateStrategy<IPoint2D> updateStrategy = new Point2DUpdateStrategy();
        /// <inheritdoc />
        public Guid Id { get; }
        /// <inheritdoc />
        public double X { get; set; } = 0;
        /// <inheritdoc />
        public double Y { get; set; } = 0;

        public Point2D(Guid id)
        {
            Id = id;
        }

        public Point2D() : this(Guid.NewGuid()) { }

        public Point2D(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public object Clone()
        {
            var newItem = new Point2D();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
