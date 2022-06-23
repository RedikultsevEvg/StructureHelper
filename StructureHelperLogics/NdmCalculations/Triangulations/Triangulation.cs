using System;
using System.Collections.Generic;
using System.Text;
using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Ndms;
using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using StructureHelperLogics.NdmCalculations.Materials;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public static class Triangulation
    {
        public static IEnumerable<INdm> GetNdms(IEnumerable<INdmPrimitive> ndmPrimitives, ITriangulationOptions options)
        {
            List<INdm> ndms = new List<INdm>();
            Dictionary<string, IPrimitiveMaterial> primitiveMaterials = GetPrimitiveMaterials(ndmPrimitives);
            Dictionary<string, IMaterial> materials = GetMaterials(primitiveMaterials, options);
            foreach (var ndmPrimitive in ndmPrimitives)
            {
                IPrimitiveMaterial primitiveMaterial = ndmPrimitive.PrimitiveMaterial;
                IMaterial material;
                if (materials.TryGetValue(primitiveMaterial.Id, out material) == false) { throw new Exception("Material dictionary is not valid"); }
                IEnumerable<INdm> localNdms = GetNdmsByPrimitive(ndmPrimitive, material);
                ndms.AddRange(localNdms);
            }
            return ndms;
        }

        private static Dictionary<string, IPrimitiveMaterial> GetPrimitiveMaterials(IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            Dictionary<string, IPrimitiveMaterial> primitiveMaterials = new Dictionary<string, IPrimitiveMaterial>();
            foreach (var ndmPrimitive in ndmPrimitives)
            {
                IPrimitiveMaterial material = ndmPrimitive.PrimitiveMaterial;
                if (!primitiveMaterials.ContainsKey(material.Id)) { primitiveMaterials.Add(material.Id, material); }
            }
            return primitiveMaterials;
        }

        private static Dictionary<string, IMaterial> GetMaterials(Dictionary<string, IPrimitiveMaterial> PrimitiveMaterials, ITriangulationOptions options)
        {
            Dictionary<string, IMaterial> materials = new Dictionary<string, IMaterial>();
            IEnumerable<string> keyCollection = PrimitiveMaterials.Keys;
            IMaterial material;
            foreach (string id in keyCollection)
            {
                IPrimitiveMaterial primitiveMaterial;
                if (PrimitiveMaterials.TryGetValue(id, out primitiveMaterial) == false) { throw new Exception("Material dictionary is not valid"); }
                material = GetMaterial(primitiveMaterial, options);
                materials.Add(id, material);
            }
            return materials;
        }

        private static IEnumerable<INdm> GetNdmsByPrimitive(INdmPrimitive primitive, IMaterial material)
        {
            List<INdm> ndms = new List<INdm>();
            ITriangulationLogicOptions options;
            ICenter center = primitive.Center;
            IShape shape = primitive.Shape;
            if (shape is IRectangle)
            {
                options = new RectangleTriangulationLogicOptions(primitive);
                ITriangulationLogic logic = new RectangleTriangulationLogic(options);
                ndms.AddRange(logic.GetNdmCollection(material));
            }
            else if (shape is IPoint)
            {
                IPoint point = shape as IPoint;
                options = new PointTriangulationLogicOptions(primitive.Center, point.Area);
                IPointTriangulationLogic logic = new PointTriangulationLogic(options);
                ndms.AddRange(logic.GetNdmCollection(material));
            }
            else { throw new Exception("Primitive type is not valid"); }
            return ndms;
        }

        private static IMaterial GetMaterial(IPrimitiveMaterial primitiveMaterial, ITriangulationOptions options)
        {
            IMaterial material;
            if (primitiveMaterial.MaterialType == MaterialTypes.Concrete) { material = GetConcreteMaterial(primitiveMaterial, options); }
            else if (primitiveMaterial.MaterialType == MaterialTypes.Reinforcement) { material = GetReinforcementMaterial(primitiveMaterial, options); }
            else { throw new Exception("Material type is invalid"); }
            return material;
        }

        private static IMaterial GetConcreteMaterial(IPrimitiveMaterial primitiveMaterial, ITriangulationOptions options)
        {
            IMaterialOptions materialOptions = new ConcreteOptions();
            SetMaterialOptions(materialOptions, primitiveMaterial, options);
            IMaterialBuilder builder = new ConcreteBuilder(materialOptions);
            IBuilderDirector director = new BuilderDirector(builder);
            return director.BuildMaterial();
        }

        private static IMaterial GetReinforcementMaterial(IPrimitiveMaterial primitiveMaterial, ITriangulationOptions options)
        {
            IMaterialOptions materialOptions = new ReinforcementOptions();
            SetMaterialOptions(materialOptions, primitiveMaterial, options);
            IMaterialBuilder builder = new ReinforcementBuilder(materialOptions);
            IBuilderDirector director = new BuilderDirector(builder);
            return director.BuildMaterial();
        }

        private static void SetMaterialOptions(IMaterialOptions materialOptions, IPrimitiveMaterial primitiveMaterial, ITriangulationOptions options)
        {
            materialOptions.Strength = primitiveMaterial.Strength;
            materialOptions.CodesType = CodesType.EC2_1990;
            if (options.LimiteState == Infrastructures.CommonEnums.LimitStates.Collapse) { materialOptions.LimitState = LimitStates.Collapse; }
            else if (options.LimiteState == Infrastructures.CommonEnums.LimitStates.ServiceAbility) { materialOptions.LimitState = LimitStates.ServiceAbility; }
            else if (options.LimiteState == Infrastructures.CommonEnums.LimitStates.Special) { materialOptions.LimitState = LimitStates.Special; }
            else { throw new Exception("LimitStateType is not valid"); }
            if (options.CalcTerm == Infrastructures.CommonEnums.CalcTerms.ShortTerm) { materialOptions.IsShortTerm = true; }
            else if (options.CalcTerm == Infrastructures.CommonEnums.CalcTerms.LongTerm) { materialOptions.IsShortTerm = false; }
            else { throw new Exception("Calculation term is not valid"); }
        }
    }
}
