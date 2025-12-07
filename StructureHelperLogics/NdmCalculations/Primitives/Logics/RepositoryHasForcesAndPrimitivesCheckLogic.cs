using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RepositoryHasForcesAndPrimitivesCheckLogic : CheckEntityLogic<IHasForcesAndPrimitives>
    {
        
        ICheckEntityLogic<IHasPrimitives> primitivesCheckLogic;
        ICheckEntityLogic<IHasPrimitives> PrimitivesCheckLogic => primitivesCheckLogic ??= new RepositoryHasPrimitivesCheckLogic(Repository) { TraceLogger = TraceLogger };
        public ICrossSectionRepository Repository { get; }

        public RepositoryHasForcesAndPrimitivesCheckLogic(ICrossSectionRepository repository)
        {
            Repository = repository;
        }

        public override bool Check()
        {
            CheckObject.ThrowIfNull(Repository);
            CheckObject.ThrowIfNull(Entity);
            bool result = true;
            PrimitivesCheckLogic.Entity = Entity;
            if (PrimitivesCheckLogic.Check() == false)
            {
                CheckResult += "\n";
                CheckResult += PrimitivesCheckLogic.CheckResult;
                result = false;
            }
            return result;
        }
    }
}
