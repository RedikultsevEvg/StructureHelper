using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class InterpolateTuplesViewModel : OkCancelViewModelBase
    {
        private RelayCommand invertForcesCommand;
        private RelayCommand copyToStartCommand;
        private RelayCommand copyToFinishCommand;

        public IDesignForceTuple StartDesignForce { get; private set; }
        public IDesignForceTuple FinishDesignForce { get; private set; }

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
        public int StepCount { get; set; }

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
            OnPropertyChanged(nameof(FinishMx));
            OnPropertyChanged(nameof(FinishMy));
            OnPropertyChanged(nameof(FinishNz));
        }

        public void RefreshStartTuple()
        {
            OnPropertyChanged(nameof(StartMx));
            OnPropertyChanged(nameof(StartMy));
            OnPropertyChanged(nameof(StartNz));
        }

        public InterpolateTuplesViewModel(IDesignForceTuple finishDesignForce, IDesignForceTuple startDesignForce=null, int stepCount = 100)
        {
            if (startDesignForce !=null)
            {
                if (startDesignForce.LimitState != finishDesignForce.LimitState) throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid);
                if (startDesignForce.CalcTerm != finishDesignForce.CalcTerm) throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid);
                StartDesignForce = startDesignForce;
            }
            else
            {
                StartDesignForce = new DesignForceTuple()
                {
                    CalcTerm = finishDesignForce.CalcTerm,
                    LimitState = finishDesignForce.LimitState,
                    ForceTuple = new ForceTuple() { Mx = 0, My = 0, Nz = 0 },
                };
            }
            FinishDesignForce = finishDesignForce;
            StepCount = stepCount;
        }
    }
}
