using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearLoadUpdateStrategy : IUpdateStrategy<IBeamSpanLoad>
    {
        private IUpdateStrategy<IConcentratedForce> concentratedForceUpdateStrategy;
        private IUpdateStrategy<IDistributedLoad> distributedLoadUpdateStrategy;
        public void Update(IBeamSpanLoad targetObject, IBeamSpanLoad sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            UpdateObjects(targetObject, sourceObject);
        }

        private void UpdateObjects(IBeamSpanLoad targetObject, IBeamSpanLoad sourceObject)
        {
            if (sourceObject is IDistributedLoad distributedSource)
            {
                if (targetObject is IDistributedLoad distributedTarget)
                {
                    distributedLoadUpdateStrategy.Update(distributedTarget, distributedSource);
                    return;
                }
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject) + ": target object is not distributed load");
            }
            if (sourceObject is IConcentratedForce concentratedSource)
            {
                if (targetObject is IConcentratedForce concentratedTarget)
                {
                    concentratedForceUpdateStrategy.Update(concentratedTarget, concentratedSource);
                    return;
                }
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject) + ": target object is not concentrated force");
            }
            throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
        }

        private void InitializeStrategies()
        {
            concentratedForceUpdateStrategy ??= new ConcentratedForceUpdateStrategy();
            distributedLoadUpdateStrategy ??= new DistributedLoadUpdateStrategy();
        }
    }
}
