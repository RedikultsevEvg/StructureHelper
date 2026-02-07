using StructureHelper.Infrastructure.UI.DataContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public interface IContourPrimitiveSet
    {
        ISelectedPrimitiveSet FieldPrimitiveSet { get; set; }
        ISelectedPrimitiveSet ShadePrimitiveSet { get; set; }
    }
}
