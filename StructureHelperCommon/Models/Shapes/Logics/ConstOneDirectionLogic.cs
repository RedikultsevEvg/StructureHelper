using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System.Collections.Generic;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class ConstOneDirectionLogic : IConvert2DPointTo3DPointLogic
    {
        private Directions constDirections;
        /// <summary>
        /// Direction, for which constant value is assigned
        /// </summary>
        public Directions ConstDirections
        {
            get => constDirections;
            set
            {
                var availableDirections = new List<Directions>() { Directions.X, Directions.Y, Directions.Z };
                if (availableDirections.Contains(value) == false)
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(value));
                }
                constDirections = value;
            }
        }
        /// <summary>
        /// Constant value for assigned direction
        /// </summary>
        public double ConstDirectionValue { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ConstOneDirectionLogic(Directions constDirection, double constValue)
        {
            ConstDirections = constDirection;
            ConstDirectionValue = constValue;
        }
        /// <inheritdoc/>
        public IPoint3D GetPoint3D(IPoint2D point2D)
        {
            TraceLogger?.AddMessage($"Logic convert point from 2D-space to 3D-space");
            IPoint3D point;
            if (ConstDirections == Directions.X)
            {
                point = new Point3D()
                {
                    X = ConstDirectionValue,
                    Y = - point2D.X,
                    Z = point2D.Y
                };
                TraceLogger?.AddMessage($"Constant direction is x-direction, so X = {point.X}");
                TraceLogger?.AddMessage($"X = ConstantValue = {point.X}");
                TraceLogger?.AddMessage($"Y = - point2D.X = {point.Y}");
                TraceLogger?.AddMessage($"Z = point2D.Y = {point.Z}");
            }
            else if (ConstDirections == Directions.Y)
            {
                point = new Point3D()
                {
                    X = point2D.X,
                    Y = ConstDirectionValue,
                    Z = point2D.Y
                };
                TraceLogger?.AddMessage($"Constant direction is Y-direction");
                TraceLogger?.AddMessage($"X = point2D.X = {point.X}");
                TraceLogger?.AddMessage($"Y = ConstantValue = {point.Y}");
                TraceLogger?.AddMessage($"Z = point2D.Y = {point.Z}");
            }
            else if (ConstDirections == Directions.Z)
            {
                point = new Point3D()
                {
                    X = point2D.Y,
                    Y = point2D.X,
                    Z = ConstDirectionValue
                };
                TraceLogger?.AddMessage($"Constant direction is Z-direction");
                TraceLogger?.AddMessage($"X = point2D.Y = {point.X}");
                TraceLogger?.AddMessage($"Y = point2D.X = {point.Y}");
                TraceLogger?.AddMessage($"Z = ConstantValue = {point.Z}");
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(ConstDirections));
            }
            return point;
        }
    }
}
