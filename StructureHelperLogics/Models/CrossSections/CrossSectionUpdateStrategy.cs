using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.WorkPlanes;
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
    public class CrossSectionUpdateStrategy : IParentUpdateStrategy<ICrossSection>
    {
        private IUpdateStrategy<ICrossSectionRepository> repositoryUpdateStrategy;
        private IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public CrossSectionUpdateStrategy(IUpdateStrategy<ICrossSectionRepository> repositoryUpdateStrategy,
            IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy)
        {
            this.repositoryUpdateStrategy = repositoryUpdateStrategy;
            this.workPlaneUpdateStrategy = workPlaneUpdateStrategy;
        }

        public CrossSectionUpdateStrategy() {   }

        public void Update(ICrossSection targetObject, ICrossSection sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (UpdateChildren == true)
            {
                UpdateRepository(targetObject, sourceObject);
                UpdateWorkPlane(targetObject, sourceObject);
            }
        }

        private void UpdateWorkPlane(ICrossSection targetObject, ICrossSection sourceObject)
        {
            if (sourceObject.WorkPlaneProperty is null)
            {
                targetObject.WorkPlaneProperty = new WorkPlaneProperty(Guid.NewGuid());
                return;
            }
            targetObject.WorkPlaneProperty ??= new WorkPlaneProperty(Guid.NewGuid());
            workPlaneUpdateStrategy ??= new WorkPlanePropertyUpdateStrategy();
            workPlaneUpdateStrategy.Update(targetObject.WorkPlaneProperty, sourceObject.WorkPlaneProperty);
        }

        private void UpdateRepository(ICrossSection targetObject, ICrossSection sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject.SectionRepository);
            CheckObject.ThrowIfNull(sourceObject.SectionRepository);
            repositoryUpdateStrategy ??= new CrossSectionRepositoryUpdateStrategy();
            repositoryUpdateStrategy.Update(targetObject.SectionRepository, sourceObject.SectionRepository);
        }
    }
}
