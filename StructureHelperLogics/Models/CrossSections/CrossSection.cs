using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSection : ICrossSection
    {
        public Guid Id { get; private set; }
        public ICrossSectionRepository SectionRepository { get; set; } = new CrossSectionRepository();

        public IWorkPlaneProperty WorkPlaneProperty { get; set; } = new WorkPlaneProperty(Guid.NewGuid());

        public CrossSection(Guid id)
        {
            Id = id;
        }

        public CrossSection() : this(Guid.NewGuid())
        {
            
        }


        public ICrossSection GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            CrossSection newItem = new(id);
            new CrossSectionUpdateStrategy().Update(newItem, this);
            if (cloneDepth == CloneDepth.Deep)
            {
                CheckObject.ThrowIfNull(SectionRepository);
                ICrossSectionRepository repository = SectionRepository.GetClone(cloneMode, cloneDepth);
                newItem.SectionRepository = repository;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneDepth) + ": cloning depth is not right");
            }
            return newItem;
        }
    }
}
