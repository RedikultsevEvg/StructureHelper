using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ForceTupleInterpolationViewModel : ViewModelBase
    {
        private RelayCommand invertForcesCommand;
        private RelayCommand copyToStartCommand;
        private RelayCommand copyToFinishCommand;
        private int stepCount;
        private IForceTuple startForceTuple;
        private IForceTuple endForceTuple;

        public IForceTuple StartDesignForce
        {
            get => startForceTuple; set
            {
                startForceTuple = value;
            }
        }
        public IForceTuple FinishDesignForce
        {
            get => endForceTuple; set
            {
                endForceTuple = value;
            }
        }

        public double StartFactor { get; set; }
        public double FinishFactor { get; set; }
        public double StepCountFactor { get; set; }

        public bool StepCountVisible { get; set; }
        public double StartMx
        {
            get => StartDesignForce.Mx;
            set
            {
                StartDesignForce.Mx = value;
                OnPropertyChanged(nameof(StartMx));
            }
        }
        public double StartMy
        {
            get => StartDesignForce.My;
            set
            {
                StartDesignForce.My = value;
                OnPropertyChanged(nameof(StartMy));
            }
        }
        public double StartNz
        {
            get => StartDesignForce.Nz;
            set
            {
                StartDesignForce.Nz = value;
                OnPropertyChanged(nameof(StartNz));
            }
        }
        public double FinishMx
        {
            get => FinishDesignForce.Mx;
            set
            {
                FinishDesignForce.Mx = value;
                OnPropertyChanged(nameof(FinishMx));
            }
        }
        public double FinishMy
        {
            get => FinishDesignForce.My;
            set
            {
                FinishDesignForce.My = value;
                OnPropertyChanged(nameof(FinishMy));
            }
        }
        public double FinishNz
        {
            get => FinishDesignForce.Nz;
            set
            {
                FinishDesignForce.Nz = value;
                OnPropertyChanged(nameof(FinishNz));
            }
        }
        public int StepCount
        {
            get => stepCount; set
            {
                stepCount = value;
                OnPropertyChanged(nameof(StepCount));
            }
        }

        public ICommand InvertForcesCommand
        {
            get => invertForcesCommand ??= new RelayCommand(o => InvertForces());
        }
        public ICommand CopyToStartCommand
        {
            get => copyToStartCommand ??= new RelayCommand(o => CopyFinishToStart());
        }
        public ICommand CopyToFinishCommand
        {
            get => copyToFinishCommand ??= new RelayCommand(o => CopyStartToFinish());
        }
        public InterpolateTuplesResult Result
        {
            get => new()
            {
                StartTuple = StartDesignForce,
                FinishTuple = FinishDesignForce,
                StepCount = StepCount
            };
        }

        private void InvertForces()
        {
            var tmpForce = StartDesignForce.Clone() as IForceTuple;
            StartDesignForce = FinishDesignForce;
            FinishDesignForce = tmpForce;
            StepCountVisible = true;
            RefreshStartTuple();
            RefreshFinishTuple();
        }

        private void CopyStartToFinish()
        {
            FinishDesignForce = StartDesignForce.Clone() as IForceTuple;
            RefreshFinishTuple();
        }

        private void CopyFinishToStart()
        {
            StartDesignForce = FinishDesignForce.Clone() as IForceTuple;
            RefreshStartTuple();
        }

        public void RefreshFinishTuple()
        {
            OnPropertyChanged(nameof(FinishDesignForce));
            OnPropertyChanged(nameof(FinishMx));
            OnPropertyChanged(nameof(FinishMy));
            OnPropertyChanged(nameof(FinishNz));
        }

        public void RefreshStartTuple()
        {
            OnPropertyChanged(nameof(StartDesignForce));
            OnPropertyChanged(nameof(StartMx));
            OnPropertyChanged(nameof(StartMy));
            OnPropertyChanged(nameof(StartNz));
        }

        public ForceTupleInterpolationViewModel(IForceTuple startForceTuple, IForceTuple endForceTuple, int stepCount = 100)
        {
            this.startForceTuple = startForceTuple;
            this.endForceTuple = endForceTuple;
            StepCount = stepCount;
            StepCountVisible = true;
        }
        public ForceTupleInterpolationViewModel()
        {
            
        }
    }
}
