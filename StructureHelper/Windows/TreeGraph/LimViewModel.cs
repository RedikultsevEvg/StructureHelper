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
        private bool isArg = false;
        public char GREATER { get; } = '\u2265';
        public char LESS { get; } = '\u2264';
        public char IN { get; } = '\u2208';
        public char LEFT_BOUND { get; } = '[';
        public char RIGHT_BOUND { get; } = ']';
        public char SEMICOLON { get; } = ';';
        private const string X = "x";
        private const string Y = "y";
        private string x_or_y_text;
        private string limitText;
        public string X_or_Y_text
        {
            get => x_or_y_text;
            set => x_or_y_text = value;
        }
        public string LimitText
        {
            get => limitText;
            set
            {
                limitText = value;
                OnPropertyChanged(nameof(LimitText));
            }
        }
        private double leftBound;
        private double rightBound;
        public double LeftBound
        {
            get => leftBound;
            set
            {
                leftBound = value;
                LimitText = $"{X_or_Y_text}" + $"{IN}" + $"{LEFT_BOUND}" + $"{value}" + $"{SEMICOLON}" + $"{rightBound}" + $"{RIGHT_BOUND}";
                OnPropertyChanged(nameof(LeftBound));
            }
        }
        public double RightBound
        {
            get => rightBound;
            set 
            {
                rightBound = value;
                LimitText = $"{X_or_Y_text}" + $"{IN}" + $"{LEFT_BOUND}" + $"{LeftBound}" + $"{SEMICOLON}" + $"{value}" + $"{RIGHT_BOUND}";
                OnPropertyChanged(nameof(RightBound));
            } 
        }
        public LimViewModel(bool isArg) 
        {
            this.isArg = isArg;
            if (isArg)
            {
                X_or_Y_text = X;
            }
            else
            {
                X_or_Y_text = Y;
            }
            LimitText = $"{X_or_Y_text}" + $"{IN}" + $"{LEFT_BOUND}" + $"{LeftBound}" + $"{SEMICOLON}" + $"{rightBound}" + $"{RIGHT_BOUND}";
        }
    }
}
