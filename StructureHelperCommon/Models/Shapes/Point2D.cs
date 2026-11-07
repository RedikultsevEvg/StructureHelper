using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;
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
            return GetClone(this, CloningMode.NewEntity);
        }

        public IPoint2D GetClone(IPoint2D sourceObject, CloningMode cloningMode)
        {
            if (cloningMode == CloningMode.Reference)
            {
                return sourceObject;
            }

            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            var newItem = new Point2D(id);
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
