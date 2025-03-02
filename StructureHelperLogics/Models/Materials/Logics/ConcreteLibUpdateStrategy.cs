using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class ConcreteLibUpdateStrategy : IUpdateStrategy<IConcreteLibMaterial>
    {
        private IUpdateStrategy<ILibMaterial> libUpdateStrategy;
        public ConcreteLibUpdateStrategy(IUpdateStrategy<ILibMaterial> libUpdateStrategy)
        {
            this.libUpdateStrategy = libUpdateStrategy;
        }
        public ConcreteLibUpdateStrategy() : this(new LibMaterialUpdateStrategy())
        {
            
        }
        public void Update(IConcreteLibMaterial targetObject, IConcreteLibMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            libUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.TensionForULS = sourceObject.TensionForULS;
            targetObject.TensionForSLS = sourceObject.TensionForSLS;
            targetObject.RelativeHumidity = sourceObject.RelativeHumidity;
            targetObject.MinAge = sourceObject.MinAge;
            targetObject.MaxAge = sourceObject.MaxAge;
        }
    }
}
