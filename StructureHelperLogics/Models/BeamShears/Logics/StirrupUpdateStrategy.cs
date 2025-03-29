using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class StirrupUpdateStrategy : IUpdateStrategy<IStirrup>
    {
        private IUpdateStrategy<IStirrupByDensity> densityUpdateStrategy;
        private IUpdateStrategy<IStirrupByRebar> uniformUpdateStrategy;
        public void Update(IStirrup targetObject, IStirrup sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject is IStirrupByDensity density)
            {
                UpdateByDensity(targetObject, density);
            }
            else if (sourceObject is IStirrupByRebar stirrupByUniformRebar)
            {
                UpdateByUniformRebar(targetObject, stirrupByUniformRebar);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }

        private void UpdateByUniformRebar(IStirrup targetObject, IStirrupByRebar stirrupByUniformRebar)
        {
            uniformUpdateStrategy ??= new StirrupByRebarUpdateStrategy();
            if (targetObject is IStirrupByRebar targetUniformRebar)
            {
                uniformUpdateStrategy.Update(targetUniformRebar, stirrupByUniformRebar);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject));
            }
        }

        private void UpdateByDensity(IStirrup targetObject, IStirrupByDensity density)
        {
            densityUpdateStrategy ??= new StirrupByDensityUpdateStrategy();
            if (targetObject is IStirrupByDensity targetDensity)
            {
                densityUpdateStrategy.Update(targetDensity, density);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject));
            }
        }
    }
}
