using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceFactoredList : IForceFactoredList
    {
        readonly IUpdateStrategy<IForceFactoredList> updateStrategy = new ForceFactoredListUpdateStrategy();
        private List<IForceCombinationList> result;
        private IGetCombinationByFactoredTupleLogic getCombinationLogic = new GetCombinationByFactoredTupleLogic();

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; } = "New Factored Load";
        public bool SetInGravityCenter { get; set; } = true;
        /// <inheritdoc/>
        public IPoint2D ForcePoint { get; set; } = new Point2D();
        /// <inheritdoc/>
        public List<IForceTuple> ForceTuples { get; } = new() { new ForceTuple()};
        /// <inheritdoc/>
        public IFactoredCombinationProperty CombinationProperty { get; set; }= new FactoredCombinationProperty(Guid.NewGuid());


        public ForceFactoredList(Guid id)
        {
            Id = id;
        }
        public ForceFactoredList() : this (Guid.NewGuid()) { }
        public object Clone()
        {
            var newItem = new ForceFactoredList();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
        /// <inheritdoc/>
        public List<IForceCombinationList> GetCombinations()
        {
            getCombinationLogic.CombinationProperty = CombinationProperty;
            getCombinationLogic.ForceActionProperty = new ForceActionProperty()
            {
                SetInGravityCenter = SetInGravityCenter,
                ForcePoint = ForcePoint
            };
            result = new();
            ForceTuples.ForEach(x => GetCombinationByForceTuple(x));
            return result;
        }

        private void GetCombinationByForceTuple(IForceTuple forceTuple)
        {
            getCombinationLogic.SourceForceTuple = forceTuple;
            result.Add(getCombinationLogic.GetCombinationList());
        }
    }
}
