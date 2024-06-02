using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Codes.Factories
{
    public static class CodeFactory
    {
        public static List<ICodeEntity> GetCodeEntities()
        {
            List<ICodeEntity> items = new List<ICodeEntity>();
            items.AddRange(GetRussianCodes());
            items.AddRange(GetEuropeanCodes());
            return items;
        }

        private static List<ICodeEntity> GetRussianCodes()
        {
            const NatSystems natSystem = NatSystems.RU;
            return new List<ICodeEntity>
            {
                new CodeEntity(new Guid("d4ab402a-ce2f-46db-8b3b-a5a66fb384e1"), natSystem)
                {
                    Name = "SP 63.13330.2018",
                    FullName = "Plain concrete and reinforced concrete structures"
                },
                new CodeEntity(new Guid("1a717049-cee7-40e0-923c-7a32a573a303"), natSystem)
                {
                    Name = "GOST 26633-2015",
                    FullName = "Heavy-weight and sand concretes. Specifications"
                },
                new CodeEntity(new Guid("c7c0f60f-2c82-45d1-8786-4c340fb5fb98"), natSystem)
                {
                    Name = "GOST 34028-2016",
                    FullName = "Reinforcing rolled products for reinforced concrete constructions. Specifications"
                }
                ,
                new CodeEntity(new Guid("d934763d-4cb4-4923-ad15-2e78b0fe3b37"), natSystem)
                {
                    Name = "GOST 53772-2010",
                    FullName = "Reinforced steel low-relaxation 7-wire strands. Specifications"
                }
            };
        }
        private static List<ICodeEntity> GetEuropeanCodes()
        {
            const NatSystems natSystem = NatSystems.EU;
            return new List<ICodeEntity>
            {
                new CodeEntity(new Guid("a72c4448-7d05-4076-9636-1a6da3bfdd40"), natSystem)
                {
                    Name = "EuroCode2-1990",
                    FullName = "Plain concrete and reinforced concrete structures"
                },
            };
        }
    }
}
