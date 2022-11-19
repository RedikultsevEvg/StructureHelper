using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class LineShape : ILineShape
    {
        /// <inheritdoc />
        public ICenter StartPoint { get; set; }
        /// <inheritdoc />
        public ICenter EndPoint { get; set; }
        /// <inheritdoc />
        public double Thickness { get; set; }

        public LineShape()
        {
            StartPoint = new Center();
            EndPoint = new Center();
            Thickness = 0;
        }
    }
}
