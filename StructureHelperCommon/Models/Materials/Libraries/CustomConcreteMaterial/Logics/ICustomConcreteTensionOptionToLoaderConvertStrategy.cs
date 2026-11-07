using LD = LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    internal interface ICustomConcreteTensionOptionToLoaderConvertStrategy : IObjectConvertStrategy<LD.CustomConcreteTensionOptions, ICustomConcreteTensionOptions>
    {
        bool WorkInTension { get; set; }
        double StrengthFactor { get; set; }
    }
}
