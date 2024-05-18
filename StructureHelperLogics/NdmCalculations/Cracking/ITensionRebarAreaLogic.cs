using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic for obtaining of summary area in tension zone of cracked cross-section
    /// </summary>
    public interface ITensionRebarAreaLogic : ILogic
    {
        
        IStrainMatrix StrainMatrix { get; set; }
        IEnumerable<RebarNdm> Rebars { get; set; }
        double GetTensionRebarArea();
    }
}
