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
        private IUpdateStrategy<ITrapezoidDistributedLoad> trapezoidUpdateStrategy;

        private IUpdateStrategy<IConcentratedForce> ConcentratedForceUpdateStrategy => concentratedForceUpdateStrategy ??= new ConcentratedForceUpdateStrategy();
        private IUpdateStrategy<IDistributedLoad> DistributedLoadUpdateStrategy => distributedLoadUpdateStrategy ??= new DistributedLoadUpdateStrategy();
        private IUpdateStrategy<ITrapezoidDistributedLoad> TrapezoidUpdateStrategy => trapezoidUpdateStrategy ??= new TrapezoidDistributedLoadUpdateStrategy();
        public void Update(IBeamSpanLoad targetObject, IBeamSpanLoad sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            UpdateObjects(targetObject, sourceObject);
        }

        private void UpdateObjects(IBeamSpanLoad targetObject, IBeamSpanLoad sourceObject)
        {
            if (sourceObject is IDistributedLoad distributedSource)
            {
                UpdateDistributedLoad(targetObject, distributedSource);
            }
            else if (sourceObject is IConcentratedForce concentratedSource)
            {
                UpdateConcentratedForce(targetObject, concentratedSource);
            }
            else if (sourceObject is ITrapezoidDistributedLoad trapezoidSource)
            {
                UpdateTrapezoidLoad(targetObject, trapezoidSource);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }

        private void UpdateTrapezoidLoad(IBeamSpanLoad targetObject, ITrapezoidDistributedLoad trapezoidSource)
        {
            if (targetObject is not ITrapezoidDistributedLoad trapezoidTargetLoad)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject) + ": target object is not trapezoid distributed load");
            }
            TrapezoidUpdateStrategy.Update(trapezoidTargetLoad, trapezoidSource);
        }

        private void UpdateConcentratedForce(IBeamSpanLoad targetObject, IConcentratedForce concentratedSource)
        {
            if (targetObject is not IConcentratedForce concentratedTarget)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject) + ": target object is not concentrated force");
            }
            ConcentratedForceUpdateStrategy.Update(concentratedTarget, concentratedSource);
        }

        private void UpdateDistributedLoad(IBeamSpanLoad targetObject, IDistributedLoad distributedSource)
        {
            if (targetObject is not IDistributedLoad distributedTarget)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(targetObject) + ": target object is not distributed load");
            }
            DistributedLoadUpdateStrategy.Update(distributedTarget, distributedSource);
        }
    }
}
