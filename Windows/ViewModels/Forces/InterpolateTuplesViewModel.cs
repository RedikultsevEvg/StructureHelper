using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class InterpolateTuplesViewModel : OkCancelViewModelBase
    {
        public IDesignForceTuple StartDesignForce { get; }
        public IDesignForceTuple FinishDesignForce { get; }
        public int StepCount { get; set; }

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
