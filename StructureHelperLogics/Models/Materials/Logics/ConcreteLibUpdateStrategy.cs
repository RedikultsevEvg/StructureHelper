using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal class ConcreteLibUpdateStrategy : IUpdateStrategy<IConcreteLibMaterial>
    {
        LibMaterialUpdateStrategy libUpdateStrategy = new LibMaterialUpdateStrategy();
        public void Update(IConcreteLibMaterial targetObject, IConcreteLibMaterial sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            libUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.TensionForULS = sourceObject.TensionForULS;
            targetObject.TensionForSLS = sourceObject.TensionForSLS;
            targetObject.RelativeHumidity = sourceObject.RelativeHumidity;
        }
    }
}
