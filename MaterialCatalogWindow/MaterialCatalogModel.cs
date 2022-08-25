using System.Collections.Generic;
using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;

namespace StructureHelper.MaterialCatalogWindow
{
    public class MaterialCatalogModel
    {
        public NamedList<MaterialDefinitionBase> ConcreteDefinitions;
        public NamedList<MaterialDefinitionBase> RebarDefinitions;

        public List<NamedList<MaterialDefinitionBase>> Materials;

        public MaterialCatalogModel()
        {
            InitializeMaterialCollections();
            Materials = new List<NamedList<MaterialDefinitionBase>>();
            Materials.Add(ConcreteDefinitions);
            Materials.Add(RebarDefinitions);
        }

        public void InitializeMaterialCollections()
        {
            InitializeConcreteDefinitions();
            InitializeRebarDefinitions();
        }

        private void InitializeRebarDefinitions()
        {
            RebarDefinitions = new NamedList<MaterialDefinitionBase>
            {
                new RebarDefinition("S240", 2, 240, 240, 1.15, 1.15),
                new RebarDefinition("S400", 2, 400, 400, 1.15, 1.15),
                new RebarDefinition("S500", 2, 500, 500, 1.15, 1.15)
            };
            RebarDefinitions.Name = "Арматура";
        }

        private void InitializeConcreteDefinitions()
        {
            ConcreteDefinitions = new NamedList<MaterialDefinitionBase>
            {
                new ConcreteDefinition("C10", 0, 10, 0, 1.3, 1.5),
                new ConcreteDefinition("C15", 0, 15, 0, 1.3, 1.5),
                new ConcreteDefinition("C20", 0, 20, 0, 1.3, 1.5),
                new ConcreteDefinition("C25", 0, 25, 0, 1.3, 1.5),
                new ConcreteDefinition("C30", 0, 30, 0, 1.3, 1.5),
                new ConcreteDefinition("C35", 0, 35, 0, 1.3, 1.5),
                new ConcreteDefinition("C40", 0, 40, 0, 1.3, 1.5),
                new ConcreteDefinition("C45", 0, 45, 0, 1.3, 1.5),
                new ConcreteDefinition("C50", 0, 50, 0, 1.3, 1.5),
                new ConcreteDefinition("C60", 0, 60, 0, 1.3, 1.5),
                new ConcreteDefinition("C70", 0, 70, 0, 1.3, 1.5),
                new ConcreteDefinition("C80", 0, 80, 0, 1.3, 1.5)
            };
            ConcreteDefinitions.Name = "Бетон";
        }
    }
}
