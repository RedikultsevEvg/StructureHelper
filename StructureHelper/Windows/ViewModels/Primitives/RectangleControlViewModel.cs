using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Primitives
{
    internal class RectangleControlViewModel : ViewModelBase
    {
        private RectangleViewPrimitive primitive;



        public RectangleControlViewModel(RectangleViewPrimitive  _primitive)
        {
            primitive = _primitive;
        }
    }
}
