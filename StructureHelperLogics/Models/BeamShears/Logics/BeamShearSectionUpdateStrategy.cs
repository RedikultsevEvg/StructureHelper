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
        public void Update(IBeamShearSection targetObject, IBeamShearSection sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            targetObject.Name = sourceObject.Name;
            shapeUpdateStrategy.Update(targetObject.Shape, sourceObject.Shape);
            targetObject.Material ??= new ConcreteLibMaterial();
            concreteUpdateStrategy.Update(targetObject.Material, sourceObject.Material);
            targetObject.CenterCover = sourceObject.CenterCover;
        }

        private void InitializeStrategies()
        {
            shapeUpdateStrategy ??= new ShapeUpdateStrategy();
            concreteUpdateStrategy ??= new ConcreteLibUpdateStrategy();
        }
    }
}
