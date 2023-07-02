using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelper.Models.Materials
{
    public interface IHeadMaterial : ISaveable, ICloneable
    {
        string Name { get; set; }
        Color Color { get; set; }
        IHelperMaterial HelperMaterial { get; set; }
        IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm);
    }
}
