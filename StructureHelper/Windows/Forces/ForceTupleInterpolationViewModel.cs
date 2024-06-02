using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ForceTupleInterpolationViewModel : ViewModelBase
    {
        private RelayCommand invertForcesCommand;
        private RelayCommand copyToStartCommand;
        private RelayCommand copyToFinishCommand;
        private int stepCount;
        private IDesignForceTuple startDesignForce;
        private IDesignForceTuple finishDesignForce;

        public IDesignForceTuple StartDesignForce
        {
            get => startDesignForce; set
            {
                startDesignForce = value;
            }
        }
        public IDesignForceTuple FinishDesignForce
        {
            get => finishDesignForce; set
            {
                finishDesignForce = value;
            }
        }

        public double StartFactor { get; set; }
        public double FinishFactor { get; set; }
        public double StepCountFactor { get; set; }

        public bool StepCountVisible { get; set; }
        public double StartMx
        {
            get => StartDesignForce.ForceTuple.Mx;
            set
            {
                StartDesignForce.ForceTuple.Mx = value;
                OnPropertyChanged(nameof(StartMx));
            }
        }
        public double StartMy
        {
            get => StartDesignForce.ForceTuple.My;
            set
            {
                StartDesignForce.ForceTuple.My = value;
                OnPropertyChanged(nameof(StartMy));
            }
        }
        public double StartNz
        {
            get => StartDesignForce.ForceTuple.Nz;
            set
            {
                StartDesignForce.ForceTuple.Nz = value;
                OnPropertyChanged(nameof(StartNz));
            }
        }
        public double FinishMx
        {
            get => FinishDesignForce.ForceTuple.Mx;
            set
            {
                FinishDesignForce.ForceTuple.Mx = value;
                OnPropertyChanged(nameof(FinishMx));
            }
        }
        public double FinishMy
        {
            get => FinishDesignForce.ForceTuple.My;
            set
            {
                FinishDesignForce.ForceTuple.My = value;
                OnPropertyChanged(nameof(FinishMy));
            }
        }
        public double FinishNz
        {
            get => FinishDesignForce.ForceTuple.Nz;
            set
            {
                FinishDesignForce.ForceTuple.Nz = value;
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
            var tmpForce = StartDesignForce.Clone() as IDesignForceTuple;
            StartDesignForce = FinishDesignForce;
            FinishDesignForce = tmpForce;
            StepCountVisible = true;
            RefreshStartTuple();
            RefreshFinishTuple();
        }

        private void CopyStartToFinish()
        {
            FinishDesignForce = StartDesignForce.Clone() as IDesignForceTuple;
            RefreshFinishTuple();
        }

        private void CopyFinishToStart()
        {
            StartDesignForce = FinishDesignForce.Clone() as IDesignForceTuple;
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

        public ForceTupleInterpolationViewModel(IDesignForceTuple finishDesignForce, IDesignForceTuple startDesignForce = null, int stepCount = 100)
        {
            if (startDesignForce != null)
            {
                CheckDesignForces(finishDesignForce, startDesignForce);
                StartDesignForce = startDesignForce;
            }
            else
            {
                GetNewDesignForce(finishDesignForce);
            }
            FinishDesignForce = finishDesignForce;
            StepCount = stepCount;
            StepCountVisible = true;
        }
        public ForceTupleInterpolationViewModel()
        {
            
        }

        private static void CheckDesignForces(IDesignForceTuple finishDesignForce, IDesignForceTuple startDesignForce)
        {
            if (startDesignForce.LimitState != finishDesignForce.LimitState)
            {
                throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid);
            }
            if (startDesignForce.CalcTerm != finishDesignForce.CalcTerm)
            {
                throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid);
            }
        }

        private void GetNewDesignForce(IDesignForceTuple finishDesignForce)
        {
            StartDesignForce = new DesignForceTuple()
            {
                CalcTerm = finishDesignForce.CalcTerm,
                LimitState = finishDesignForce.LimitState,
                ForceTuple = new ForceTuple()
                {
                    Mx = 0,
                    My = 0,
                    Nz = 0
                },
            };
        }
    }
}
