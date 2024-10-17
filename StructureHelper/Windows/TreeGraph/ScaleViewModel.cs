using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.TreeGraph
{
    public class ScaleViewModel : ViewModelBase
    {
        private double scaleFactor;
        private string scaleText;
        public double ScaleFactor
        {
            get => scaleFactor; 
            set => scaleFactor = value;
        }
        public string ScaleText
        {
            get => scaleText;
            set => scaleText = value;
        }
        public ScaleViewModel() 
        { 
        }
    }
}
