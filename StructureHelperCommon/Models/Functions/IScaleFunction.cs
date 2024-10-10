using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions
{
    public interface IScaleFunction : IFunctionDecorator
    {
        public void Scale();
    }
}
