﻿using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class DesignForceTuple : IDesignForceTuple
    {
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IForceTuple ForceTuple { get; set; }

        public DesignForceTuple(LimitStates limitState, CalcTerms calcTerm) : this()
        {
            LimitState = limitState;
            CalcTerm = calcTerm;
        }

        public DesignForceTuple()
        {
            ForceTuple = new ForceTuple();
        }

        public object Clone()
        {
            var newTuple = new DesignForceTuple(this.LimitState, this.CalcTerm);
            newTuple.ForceTuple = this.ForceTuple.Clone() as IForceTuple;
            return newTuple;
        }
    }
}
