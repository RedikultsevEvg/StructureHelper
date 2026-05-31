using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.Materials
{
    public class UserMaterialPropertyToLoaderMaterialConvertLogic : IObjectConvertStrategy<IMaterial, IUserMaterialProperty>
    {
        public double PositiveStressFactor { get; set; } = 1.0;
        public double NegativeStressFactor { get; set; } = 1.0;
        public IMaterial Convert(IUserMaterialProperty source)
        {
            Material material = new();
            material.InitModulus = source.YoungModulus;
            material.LimitPositiveStrain = source.LimitPositiveStrain;
            material.LimitNegativeStrain = source.LimitNegativeStrain;
            List<IStressStrainPair> pointList = [];
            foreach (var item in source.StressStrainPairs)
            {
                double factor;
                if (item.Stress >= 0)
                {
                    factor = PositiveStressFactor;
                }
                else
                {
                    factor = NegativeStressFactor;
                }
                pointList.Add(new StressStrainPair() { Strain = item.Strain, Stress = item.Stress * factor});
            }
            MultyLinearStressStrainFullDiagram diagram = new(pointList);
            diagram.MaxStrain = source.StressStrainPairs.Max(x => x.Strain);
            diagram.MinStrain = source.StressStrainPairs.Min(x => x.Strain);
            material.Diagram = diagram.GetStressByStrain;
            return material;
        }
    }
}
