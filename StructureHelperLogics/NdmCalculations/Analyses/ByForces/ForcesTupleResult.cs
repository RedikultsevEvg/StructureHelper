using LoaderCalculator.Data.ResultData;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForcesTupleResult : IForcesTupleResult
    {
        public bool IsValid { get; set; }
        public IDesignForceTuple DesignForceTuple { get; set; }
        /// <summary>
        /// Text of result of calculations
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Keep result of calculations from ndm-library
        /// </summary>
        public ILoaderResults LoaderResults { get; set; }

        public ForcesTupleResult()
        {
            DesignForceTuple = new DesignForceTuple();
        }
    }
}
