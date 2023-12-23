using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class RoundSurroundProc : ISurroundProc
    {
        private List<IPoint2D> surroundList;

        public SurroundData SurroundData { get; set; }
        public int PointCount { get; set; }

        public RoundSurroundProc()
        {
            SurroundData = new();
        }
        public List<IPoint2D> GetPoints()
        {
            var xRadius = (SurroundData.XMax - SurroundData.XMin) / 2;
            var yRadius = (SurroundData.YMax - SurroundData.YMin) / 2;
            var xCenter = (SurroundData.XMax + SurroundData.XMin) / 2;
            var yCenter = (SurroundData.YMax + SurroundData.YMin) / 2;
            surroundList = new();
            var pointCount = Convert.ToInt32(Math.Ceiling(PointCount / 4d) * 4d);
            double angleStep = 2d * Math.PI / pointCount;
            double angle;
            for (int i = 0; i < pointCount; i++)
            {
                double x, y;
                angle = angleStep * i;
                x = xRadius * Math.Cos(angle) + xCenter;
                y = yRadius * Math.Sin(angle) + yCenter;
                surroundList.Add(new Point2D() { X = x, Y = y });
            }
            surroundList.Add(surroundList[0].Clone() as IPoint2D);
            return surroundList;
        }
    }
}
