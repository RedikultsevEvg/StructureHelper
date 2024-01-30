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
        /// Direction, for which canstant value is assigned
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
        public ConstOneDirectionLogic(Directions constDirection, double constValue)
        {
            ConstDirections = constDirection;
            ConstDirectionValue = constValue;
        }
        /// <inheritdoc/>
        public IPoint3D GetPoint3D(IPoint2D point2D)
        {
            IPoint3D point;
            if (ConstDirections == Directions.X)
            {
                point = new Point3D() { X = ConstDirectionValue, Y = - point2D.X, Z = point2D.Y };
            }
            else if (ConstDirections == Directions.Y)
            {
                point = new Point3D() { X = point2D.X, Y = ConstDirectionValue, Z = point2D.Y };
            }
            else if (ConstDirections == Directions.Z)
            {
                point = new Point3D() { X = point2D.Y, Y = point2D.X, Z = ConstDirectionValue };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(ConstDirections));
            }
            return point;
        }
    }
}
