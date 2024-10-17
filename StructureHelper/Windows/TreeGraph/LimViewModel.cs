using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.TreeGraph
{
    public class LimViewModel : ViewModelBase
    {
        public char GREATER { get; } = '\u2A7E';
        public char LESS { get; } = '\u2264';
        public char IN { get; } = '\u2208';
        public char LEFT_BOUND { get; } = '[';
        public char RIGHT_BOUND { get; } = ']';
        public char SEMICOLON { get; } = ';';
        private string x_or_y_text = "x";
        private string limitText;
        public string X_or_Y_text
        {
            get => x_or_y_text;
            set => x_or_y_text = value;
        }
        public string LimitText
        {
            get => limitText;
            set => limitText = value;
        }
        private double leftBound;
        private double rightBound;
        public double LeftBound
        {
            get => leftBound;
            set => leftBound = value;
        }
        public double RightBound
        {
            get => rightBound;
            set => rightBound = value;
        }
        public LimViewModel() 
        {
            LimitText = $"{X_or_Y_text}" + $"{IN}" + $"{LEFT_BOUND}" + $"{LeftBound}" + $"{SEMICOLON}" + $"{rightBound}" + $"{RIGHT_BOUND}";
        }
    }
}
