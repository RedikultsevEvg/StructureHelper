using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class RectSurroundProc : ISurroundProc
    {
        private List<IPoint2D> surroundList;

        public SurroundData SurroundData { get; set; }

        public List<IPoint2D> GetPoints()
        {
            var xRadius = (SurroundData.XMax - SurroundData.XMin) / 2;
            var yRadius = (SurroundData.YMax - SurroundData.YMin) / 2;
            surroundList = new();
            var pointCount = Convert.ToInt32(Math.Ceiling(SurroundData.PointCount / 8d));
            double xStep = xRadius / pointCount;
            double yStep = yRadius / pointCount;
            double x, y;
            x = SurroundData.XMax;
            for (int i = 0; i < pointCount * 2; i++)
            {
                y = SurroundData.YMin + yStep * i;
                surroundList.Add(new Point2D() { X = x, Y = y });
            }
            y = SurroundData.YMax;
            for (int i = 0; i < pointCount * 2; i++)
            {
                x = SurroundData.XMax - xStep * i;
                surroundList.Add(new Point2D() { X = x, Y = y });
            }
            x = SurroundData.XMin;
            for (int i = 0; i < pointCount * 2; i++)
            {
                y = SurroundData.YMax - yStep * i;
                surroundList.Add(new Point2D() { X = x, Y = y });
            }
            y = SurroundData.YMin;
            for (int i = 0; i < pointCount * 2; i++)
            {
                x = SurroundData.XMin + xStep * i;
                surroundList.Add(new Point2D() { X = x, Y = y });
            }
            surroundList.Add(surroundList[0].Clone() as IPoint2D);
            return surroundList;
        }
    }
}
