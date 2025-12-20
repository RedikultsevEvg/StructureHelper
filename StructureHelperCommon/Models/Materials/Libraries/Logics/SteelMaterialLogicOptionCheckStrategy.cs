using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries.Logics
{
    public class SteelMaterialLogicOptionCheckStrategy : CheckEntityLogic<ISteelMaterialLogicOption>
    {
        public override bool Check()
        {
            bool result = true;
            if (Entity is null)
            {
                throw new StructureHelperNullReferenceException("Option is null");
            }
            if (Entity.SafetyFactors is null)
            {
                TraceMessage("Option safety factors collection is null");
                result = false;
            }
            if (Entity.ThicknessFactor < 0 )
            {
                TraceMessage("Option thickness factor is null");
                result = false;
            }
            if (Entity.WorkConditionFactor < 0)
            {
                TraceMessage("Option work condition factor is null");
                result = false;
            }
            if (Entity.UlsFactor < 0)
            {
                TraceMessage("Option ULS factor is null");
                result = false;
            }
            if (Entity.SlsFactor < 0)
            {
                TraceMessage("Option SLS factor is null");
                result = false;
            }
            return result;
        }
    }
}
