using LD = LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials
{
    internal class CustomConcreteCompressionOptionsToLoaderConvertStrategy : IObjectConvertStrategy<LD.CustomConcreteCompressionOptions, ICustomConcreteCompressionOptions>
    {   
        public LD.CustomConcreteCompressionOptions Convert(ICustomConcreteCompressionOptions source)
        {
            CheckObject.ThrowIfNull(source, "source");
            LD.CustomConcreteCompressionOptions options = new()
            {
                PeakStress = source.PeakStress,
                PeakStrain = source.PeakStrain,
                LimitStrain = source.LimitStrain,
                DescendingScaleFactor = source.DescendingScaleFactor
            };
            return options;
        }
    }
}
