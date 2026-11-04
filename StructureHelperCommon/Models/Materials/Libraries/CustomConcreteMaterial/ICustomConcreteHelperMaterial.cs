using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface ICustomConcreteHelperMaterial : IHelperMaterial, ICrackedMaterial
    {
        List<ICustomConcreteModel> MaterialModels { get; }
    }
}
