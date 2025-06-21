using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionUpdateStrategy : IUpdateStrategy<IBeamShearSection>
    {
        private IUpdateStrategy<IShape> shapeUpdateStrategy;
        private IUpdateStrategy<IConcreteLibMaterial> concreteUpdateStrategy;
        private IUpdateStrategy<IReinforcementLibMaterial> reinforcementUpdateStrategy;
        public void Update(IBeamShearSection targetObject, IBeamShearSection sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            targetObject.Name = sourceObject.Name;
            targetObject.ReinforcementArea = sourceObject.ReinforcementArea;
            shapeUpdateStrategy.Update(targetObject.Shape, sourceObject.Shape);
            targetObject.ConcreteMaterial ??= new ConcreteLibMaterial();
            concreteUpdateStrategy.Update(targetObject.ConcreteMaterial, sourceObject.ConcreteMaterial);
            targetObject.ReinforcementMaterial ??= new ReinforcementLibMaterial(Guid.NewGuid());
            reinforcementUpdateStrategy.Update(targetObject.ReinforcementMaterial, sourceObject.ReinforcementMaterial);
            targetObject.CenterCover = sourceObject.CenterCover;
        }

        private void InitializeStrategies()
        {
            shapeUpdateStrategy ??= new ShapeUpdateStrategy();
            concreteUpdateStrategy ??= new ConcreteLibUpdateStrategy();
            reinforcementUpdateStrategy ??= new ReinforcementLibUpdateStrategy();
        }
    }
}
