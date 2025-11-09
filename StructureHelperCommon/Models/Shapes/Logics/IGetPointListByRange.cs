using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IGetPointListByRange
    {
        int StepNumber { get; set; }
        List<IPoint2D> GetPoints(IPoint2DRange range);
    }
}
