using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public interface IHasActionByResult
    {
        Action<IResult> ActionToOutputResults { get; set; }
    }
}
