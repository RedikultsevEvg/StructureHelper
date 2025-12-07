using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RepositoryHasPrimitivesCheckLogic : CheckEntityLogic<IHasPrimitives>
    {
        public ICrossSectionRepository Repository { get; }

        public RepositoryHasPrimitivesCheckLogic(ICrossSectionRepository crossSectionRepository)
        {
            Repository = crossSectionRepository;
        }

        public override bool Check()
        {
            bool result = true;
            CheckObject.ThrowIfNull(Repository);
            CheckObject.ThrowIfNull(Entity);
            foreach (var primitive in Entity.Primitives)
            {
                if (! Repository.Primitives.Contains(primitive))
                {
                    TraceMessage($"Repository does not contain primitive Id = {primitive.Id}, Name = {primitive.Name}");
                    result = false;
                }
            }
            return result;
        }
    }
}
