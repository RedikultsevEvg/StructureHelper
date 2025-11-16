using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionUpdateStrategy : IParentUpdateStrategy<IBeamShearSection>
    {
        private IUpdateStrategy<IShape> shapeUpdateStrategy;
        private IUpdateStrategy<IConcreteLibMaterial> concreteUpdateStrategy;
        private IUpdateStrategy<IReinforcementLibMaterial> reinforcementUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public void Update(IBeamShearSection targetObject, IBeamShearSection sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            Check(targetObject, sourceObject);
            InitializeStrategies();
            targetObject.Name = sourceObject.Name;
            targetObject.ReinforcementArea = sourceObject.ReinforcementArea;
            targetObject.CenterCover = sourceObject.CenterCover;
            if (UpdateChildren)
            {
                shapeUpdateStrategy.Update(targetObject.Shape, sourceObject.Shape);
                targetObject.ConcreteMaterial ??= new ConcreteLibMaterial(Guid.NewGuid());
                concreteUpdateStrategy.Update(targetObject.ConcreteMaterial, sourceObject.ConcreteMaterial);
                targetObject.ReinforcementMaterial ??= new ReinforcementLibMaterial(Guid.NewGuid());
                reinforcementUpdateStrategy.Update(targetObject.ReinforcementMaterial, sourceObject.ReinforcementMaterial);
            }
        }

        private static void Check(IBeamShearSection targetObject, IBeamShearSection sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject.Shape, "Source object shape");
            CheckObject.ThrowIfNull(targetObject.Shape, "Target object shape");
            CheckObject.ThrowIfNull(sourceObject.ConcreteMaterial, "Source concrete material");
            CheckObject.ThrowIfNull(targetObject.ConcreteMaterial, "Target concrete material");
            CheckObject.ThrowIfNull(sourceObject.ReinforcementMaterial, "Source reinforcement material");
            CheckObject.ThrowIfNull(targetObject.ReinforcementMaterial, "Target reinforcement material");
        }

        private void InitializeStrategies()
        {
            shapeUpdateStrategy ??= new ShapeUpdateStrategy();
            concreteUpdateStrategy ??= new ConcreteLibUpdateStrategy();
            reinforcementUpdateStrategy ??= new ReinforcementLibUpdateStrategy();
        }
    }
}
