using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.BeamShears
{
    internal class ActionToGraphicPrimitiveConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearAction>
    {
        private IInclinedSection inclinedSection;
        private List<IGraphicalPrimitive> graphicalPrimitives;
        private double maxConcentratedForceValue = 1e6;
        private double maxDistributedLoadValue = 1e6;

        public ActionToGraphicPrimitiveConvertLogic(IInclinedSection inclinedSection)
        {
            this.inclinedSection = inclinedSection;
        }

        public List<IGraphicalPrimitive> Convert(IBeamShearAction source)
        {
            SetMaxForce(source);
            SetMaxDistributedForce(source);
            graphicalPrimitives = new();
            var supportInternalAction = source.SupportAction.SupportForce.ForceTuple;
            graphicalPrimitives.Add(GetConcentratedForcePrimitive(supportInternalAction, "Support reaction"));
            foreach (var item in source.SupportAction.ShearLoads)
            {
                if (item is IDistributedLoad distributedLoad)
                {
                    graphicalPrimitives.Add(GetDistributedLoadPrimitive(distributedLoad));
                }
                else if (item is IConcentratedForce concentratedForce)
                {
                    graphicalPrimitives.Add(GetConcentratedForcePrimitive(concentratedForce));
                }
                else if (item is ITrapezoidDistributedLoad trapezoid)
                {
                    graphicalPrimitives.Add(GetTrapezoidPrimitive(trapezoid));
                }
            }
            return graphicalPrimitives;
        }

        private IGraphicalPrimitive GetTrapezoidPrimitive(ITrapezoidDistributedLoad trapezoid)
        {
            TrapezoidDistributedLoadPrimitive trapezoidPrimitive = new(trapezoid, inclinedSection) { MaxForce = maxDistributedLoadValue};
            return trapezoidPrimitive;
        }

        private void SetMaxDistributedForce(IBeamShearAction source)
        {
            var forceList = source.SupportAction.ShearLoads
                .Where(x => x is IDistributedLoad)
                .Select(x => Math.Abs((x as IDistributedLoad).LoadValue.Qy))
                .ToList();

            forceList.AddRange(source.SupportAction.ShearLoads
                .Where(x => x is ITrapezoidDistributedLoad)
                .Select(x => Math.Abs((x as ITrapezoidDistributedLoad).StartLoadValue.Qy)));

            forceList.AddRange(source.SupportAction.ShearLoads
                .Where(x => x is ITrapezoidDistributedLoad)
                .Select(x => Math.Abs((x as ITrapezoidDistributedLoad).EndLoadValue.Qy)));
            if (! forceList.Any()) { return; }
            maxDistributedLoadValue = forceList.Max();
        }

        private IGraphicalPrimitive GetDistributedLoadPrimitive(IDistributedLoad distributedLoad)
        {
            DistributedLoadPrimitive distributedLoadPrimitive = new(distributedLoad, inclinedSection) { MaxForce = maxDistributedLoadValue };
            return distributedLoadPrimitive;
        }

        private ConcentratedForcePrimitive GetConcentratedForcePrimitive(IForceTuple supportInternalAction, string name)
        {
            ConcentratedForce force = new(Guid.Empty) { Name = name };
            force.ForceValue.Qy = supportInternalAction.Qy;
            force.RelativeLoadLevel = -0.5;
            force.ForceCoordinate = 0;
            ConcentratedForcePrimitive concentratedForcePrimitive = new(force, inclinedSection) { MaxForce = maxConcentratedForceValue};
            return concentratedForcePrimitive;
        }

        private ConcentratedForcePrimitive GetConcentratedForcePrimitive(IConcentratedForce concentratedForce)
        {
            ConcentratedForcePrimitive concentratedForcePrimitive = new(concentratedForce, inclinedSection) { MaxForce = maxConcentratedForceValue };
            return concentratedForcePrimitive;
        }

        private void SetMaxForce(IBeamShearAction source)
        {
            var forceList = source.SupportAction.ShearLoads
                .Where(x => x is IConcentratedForce)
                .Select(x => Math.Abs((x as IConcentratedForce).ForceValue.Qy))
                .ToList();
            forceList.Add(Math.Abs(source.ExternalForce.ForceTuple.Nz));
            forceList.Add(Math.Abs(source.SupportAction.SupportForce.ForceTuple.Qy));
            if (!forceList.Any()) { return; }
            maxConcentratedForceValue = forceList.Max();
        }
    }
}
