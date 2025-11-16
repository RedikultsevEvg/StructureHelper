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
        private IUpdateStrategy<IStirrupGroup> groupUpdateStrategy;
        private IUpdateStrategy<IStirrupByInclinedRebar> inclinedUpdateStrategy;

        public void Update(IStirrup targetObject, IStirrup sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject is IStirrupGroup group)
            {
                UpdateGroup(targetObject, group);
            }
            else if (sourceObject is IStirrupByRebar stirrupByUniformRebar)
            {
                UpdateByUniformRebar(targetObject, stirrupByUniformRebar);
            }
            else if (sourceObject is IStirrupByDensity density)
            {
                UpdateByDensity(targetObject, density);
            }
            else if (sourceObject is IStirrupByInclinedRebar inclinedRebar)
            {
                UpdateByInclinedRebar(targetObject, inclinedRebar);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }

        private void UpdateByInclinedRebar(IStirrup targetObject, IStirrupByInclinedRebar surceInclinedRebar)
        {
            if (targetObject is not IStirrupByInclinedRebar targetInclinedRebar)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject));
            }
            inclinedUpdateStrategy ??= new StirrupByInclinedRebarUpdateStrategy();
            inclinedUpdateStrategy.Update(targetInclinedRebar, surceInclinedRebar);
        }

        private void UpdateGroup(IStirrup targetObject, IStirrupGroup sourceGroup)
        {
            if (targetObject is not IStirrupGroup targetGroup)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject));
            }
            groupUpdateStrategy ??= new StirrupGroupUpdateStrategy();
            groupUpdateStrategy.Update(targetGroup, sourceGroup);
        }

        private void UpdateByUniformRebar(IStirrup targetObject, IStirrupByRebar stirrupByUniformRebar)
        {
            if (targetObject is not IStirrupByRebar targetUniformRebar)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject));
            }
            uniformUpdateStrategy ??= new StirrupByRebarUpdateStrategy();
            uniformUpdateStrategy.Update(targetUniformRebar, stirrupByUniformRebar);
        }

        private void UpdateByDensity(IStirrup targetObject, IStirrupByDensity density)
        {
            if (targetObject is not IStirrupByDensity targetDensity)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject));
            }
            densityUpdateStrategy ??= new StirrupByDensityUpdateStrategy();
            densityUpdateStrategy.Update(targetDensity, density);
        }
    }
}
