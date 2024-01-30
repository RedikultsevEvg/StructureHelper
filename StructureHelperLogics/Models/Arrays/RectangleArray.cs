using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Arrays
{
    internal class RectangleArray : ISaveable, IGeometryArray, IRectangleShape
    {
        private int countX;
        private int countY;
        private double width;
        private double height;
        private double offset;
        private double angle;

        public Guid Id { get; }
        public string Name { get; set; }
        public IPoint2D Center { get; set; }
        public double RotationAngle { get; set; }
        public double Width
        {
            get => width;
            set
            {
                var fieldName = nameof(Width);
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": {fieldName}");
                }
                width = value;
            }
        }
        public double Height
        {
            get => height;
            set
            {
                var fieldName = nameof(Height);
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": {fieldName}");
                }
                height = value;
            }
        }
        public double Angle
        {
            get => angle;
            set
            {
                var fieldName = nameof(Angle);
                var minAngle =  - 2d * Math.PI;
                var maxAngle = 2d * Math.PI;
                if (value < minAngle || value > maxAngle)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": {fieldName}");
                }
                angle = value;
            }
        }
        public int CountX
        {
            get => countX;
            set
            {
                var fieldName = nameof(countX);
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": {fieldName}");
                }
                countX = value;
            }
        }
        public int CountY
        {
            get => countY;
            set
            {
                var fieldName = nameof(CountY);
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": {fieldName}");
                }
                countY = value;
            }
        }
        public double Offset
        {
            get => offset;
            set
            {
                var fieldName = nameof(Width);
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": {fieldName}");
                }
                offset = value;
            }
        }
        public bool FillInside {  get; set; }
        public List<IGeometryArrayMember> Children { get; }

        public RectangleArray(Guid id)
        {
            Id = id;
            Center = new Point2D();
            Width = 0.4d;
            Height = 0.4d;
            CountX = 3;
            CountY = 3;
            FillInside = false;
        }

        public RectangleArray() : this(new Guid())
        {
            Name = "New Rectangle Array";
        }

        public List<IGeometryArrayMember> Explode()
        {
            throw new NotImplementedException();
        }

        public List<IGeometryArrayMember> GetAllChildren()
        {
            throw new NotImplementedException();
        }
    }
}
