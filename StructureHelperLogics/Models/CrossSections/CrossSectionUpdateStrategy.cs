using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionUpdateStrategy : IUpdateStrategy<ICrossSection>
    {
        private IUpdateStrategy<ICrossSectionRepository> repositoryUpdateStrategy;

        public CrossSectionUpdateStrategy(IUpdateStrategy<ICrossSectionRepository> repositoryUpdateStrategy)
        {
            this.repositoryUpdateStrategy = repositoryUpdateStrategy;
        }

        public CrossSectionUpdateStrategy() : this (
            new CrossSectionRepositoryUpdateStrategy()
            )
        {
            
        }

        public void Update(ICrossSection targetObject, ICrossSection sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.SectionRepository.Calculators.Clear();
            targetObject.SectionRepository.Primitives.Clear();
            targetObject.SectionRepository.ForceActions.Clear();
            targetObject.SectionRepository.HeadMaterials.Clear();
            repositoryUpdateStrategy.Update(targetObject.SectionRepository, sourceObject.SectionRepository);
        }
    }
}
