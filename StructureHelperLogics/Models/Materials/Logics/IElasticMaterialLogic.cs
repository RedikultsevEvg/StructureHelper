using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal interface IElasticMaterialLogic
    {
        IMaterial GetLoaderMaterial(IElasticMaterial material, LimitStates limitState, CalcTerms calcTerm, double factor = 1d);
    }
}
