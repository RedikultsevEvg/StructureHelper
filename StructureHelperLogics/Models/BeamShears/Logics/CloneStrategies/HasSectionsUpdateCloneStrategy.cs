using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasSectionsUpdateCloneStrategy : IUpdateStrategy<IHasBeamShearSections>
    {
        private readonly ICloningStrategy cloningStrategy;

        public HasSectionsUpdateCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasBeamShearSections targetObject, IHasBeamShearSections sourceObject)
        {
            CheckObject.IsNull(cloningStrategy);
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Sections.Clear();
            foreach (var item in sourceObject.Sections)
            {
                IBeamShearSection newSection = cloningStrategy.Clone(item);
                targetObject.Sections.Add(newSection);
            }
        }
    }
}
