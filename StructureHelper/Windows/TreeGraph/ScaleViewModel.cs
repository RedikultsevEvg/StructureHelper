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
        private bool isArg = false;
        private double scaleFactor;
        private string scaleText;
        private const string X_DEFAULT_SCALE_TEXT = "y=f(sx)";
        private const string Y_DEFAULT_SCALE_TEXT = "y=sf(x)";
        public double ScaleFactor
        {
            get => scaleFactor; 
            set
            {
                scaleFactor = value;
                if (isArg)
                {
                    ScaleText = $"y=f({value}x)";
                }
                else
                {
                    ScaleText = $"y={value}f(x)";
                }
                OnPropertyChanged(nameof(ScaleFactor));
            }
        }
        public string ScaleText
        {
            get => scaleText;
            set
            {
                scaleText = value;
                OnPropertyChanged(nameof(ScaleText));
            }        
        }
        public ScaleViewModel(bool isArg) 
        {
            this.isArg = isArg;
            if (isArg)
            {
                ScaleText = $"{X_DEFAULT_SCALE_TEXT}";
            }
            else
            {
                ScaleText= $"{Y_DEFAULT_SCALE_TEXT}";
            } 
        }
    }
}
