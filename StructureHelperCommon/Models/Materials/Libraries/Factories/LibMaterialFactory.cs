using System;
using System.Collections.Generic;
using System.Linq;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Codes;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public static class LibMaterialFactory
    {
        public static List<ILibMaterialEntity> GetLibMaterials()
        {
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            if (ProgramSetting.NatSystem == NatSystems.RU)
            {
                libMaterials.AddRange(GetConcreteSP63());
                libMaterials.AddRange(GetReinforcementSP63());
            }
            else if (ProgramSetting.NatSystem == NatSystems.EU)
            {
                libMaterials.AddRange(GetConcreteEurocode());
                libMaterials.AddRange(GetReinforcementEurocode());
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": {ProgramSetting.NatSystem}");
            }
            return libMaterials;
        }

        private static IEnumerable<ILibMaterialEntity> GetConcreteEurocode()
        {
            ICodeEntity code = ProgramSetting.CodesList.Where(x => x.Name == "EuroCode2-1990").Single();
            var codeType = CodeTypes.EuroCode_2_1990;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>
            {
                new ConcreteMaterialEntity(new Guid("145f3994-347b-466e-9c26-c7a8bf4a207a"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C12",
                    MainStrength = 12e6
                },
                new ConcreteMaterialEntity(new Guid("f264ef97-ebbe-4c0b-b68e-905feb1e210e"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C20",
                    MainStrength = 20e6
                },
                new ConcreteMaterialEntity(new Guid("b0d9df4d-f601-473e-8e52-05ef82b2d974"))
                { 
                    CodeType = codeType,
                    Code = code,
                    Name = "C30",
                    MainStrength = 30e6 },
                new ConcreteMaterialEntity(new Guid("196dac5f-42b6-4a43-ab24-8cd5fe8af0a4"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C40",
                    MainStrength = 40e6
                },
                new ConcreteMaterialEntity(new Guid("89e2ae9c-43e5-425f-93c6-f4b42e9916bd"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C50",
                    MainStrength = 50e6
                },
                new ConcreteMaterialEntity(new Guid("0aea6c0d-6d49-4f61-a1c5-c599af73df76"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C60",
                    MainStrength = 60e6 },
                new ConcreteMaterialEntity(new Guid("a4fb66f8-6689-489e-ab40-adab1e90ab14"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C70",
                    MainStrength = 70e6
                },
                new ConcreteMaterialEntity(new Guid("b5c36b22-ebb9-45c6-88cf-bb636187a2ed"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "C80",
                    MainStrength = 80e6
                }
            };
            return libMaterials;
        }
        private static IEnumerable<ILibMaterialEntity> GetReinforcementEurocode()
        {
            ICodeEntity code = ProgramSetting.CodesList.Where(x => x.Name == "EuroCode2-1990").Single();
            var codeType = CodeTypes.EuroCode_2_1990;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>
            {
                new ReinforcementMaterialEntity(new Guid("5413ba46-9bad-4cb3-a129-4e1a09373fd9"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "S240",
                    MainStrength = 240e6
                },
                new ReinforcementMaterialEntity(new Guid("c60c8296-82bd-4bf8-8bb5-d0cc532e7372"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "S400",
                    MainStrength = 400e6
                },
                new ReinforcementMaterialEntity(new Guid("0efb56bf-dc7f-4970-86e7-ddefb5ea7b93"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "S500",
                    MainStrength = 500e6
                }
            };
            return libMaterials;
        }
        private static IEnumerable<ILibMaterialEntity> GetConcreteSP63()
        {
            ICodeEntity code = ProgramSetting.CodesList
                .Where(x => x.Name == "GOST 26633-2015")
                .Single();
            var codeType = CodeTypes.SP63_2018;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>
            {
                new ConcreteMaterialEntity(new Guid("c63ce3b3-af54-44aa-bc06-130e6b6450ff"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B5",
                    MainStrength = 5e6
                },
                new ConcreteMaterialEntity(new Guid("9b679822-0332-4504-8435-c7e718cdb6f4"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B7,5",
                    MainStrength = 7.5e6
                },
                new ConcreteMaterialEntity(new Guid("9339af2b-46da-4354-a62e-fa330f46c165"))
                {
                    CodeType = codeType,
                    Name = "B10",
                    Code = code,
                    MainStrength = 10e6,
                },
                new ConcreteMaterialEntity(new Guid("1cdc3598-c67b-4e35-89ac-3f7c0a9db167"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B15",
                    MainStrength = 15e6
                },
                new ConcreteMaterialEntity(new Guid("f1d05405-2fd7-465e-82fc-d69f74e482aa"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B20",
                    MainStrength = 20e6
                },
                new ConcreteMaterialEntity(new Guid("27ca419d-cff3-4f7f-82af-d577bb343651"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B25",
                    MainStrength = 25e6
                },
                new ConcreteMaterialEntity(new Guid("2f5b70b9-f4c1-470d-ac27-a39a7093b6ea"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B30",
                    MainStrength = 30e6
                },
                new ConcreteMaterialEntity(new Guid("edd16698-cbe8-43ba-b249-7bab99fa0163"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B35",
                    MainStrength = 35e6
                },
                new ConcreteMaterialEntity(new Guid("32614a91-fc85-4690-aa82-af45e00f7638"))
                {
                    CodeType = codeType,
                    Code = code, 
                    Name = "B40",
                    MainStrength = 40e6
                },
                new ConcreteMaterialEntity(new Guid("6182b496-9d80-4323-8b1e-7347923d7ceb"))
                {
                    CodeType = codeType,
                    Code = code, 
                    Name = "B50",
                    MainStrength = 50e6
                },
                new ConcreteMaterialEntity(new Guid("96217bf1-564c-4150-afd6-9fe661c2e121"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "B60",
                    MainStrength = 60e6 }
            };
            return libMaterials;
        }
        private static IEnumerable<ILibMaterialEntity> GetReinforcementSP63()
        {
            var codeType = CodeTypes.SP63_2018;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            libMaterials.AddRange(AddGOST34028(codeType));
            libMaterials.AddRange(AddGOST53772(codeType));
            return libMaterials;
        }

        private static List<ILibMaterialEntity> AddGOST34028(CodeTypes codeType)
        {
            var code = ProgramSetting.CodesList
                .Where(x => x.Name == "GOST 34028-2016")
                .Single();
            List<ILibMaterialEntity> range = new List<ILibMaterialEntity>
            {
                new ReinforcementMaterialEntity(new Guid("c47ebbd7-2e0c-4247-81b6-dc3fbd064bab"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "A240",
                    MainStrength = 240e6
                },
                new ReinforcementMaterialEntity(new Guid("ea422282-3465-433c-9b93-c5bbfba5a904"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "A400",
                    MainStrength = 400e6
                },
                new ReinforcementMaterialEntity(new Guid("045b54b1-0bbf-41fd-a27d-aeb20f600bb4"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "A500",
                    MainStrength = 500e6
                }
            };
            return range;
        }
        private static List<ILibMaterialEntity> AddGOST53772(CodeTypes codeType)
        {
            var code = ProgramSetting.CodesList
                .Where(x => x.Name == "GOST 53772-2010")
                .Single();
            List<ILibMaterialEntity> range = new List<ILibMaterialEntity>()
            {
                new ReinforcementMaterialEntity(new Guid("1b44e9eb-d19d-4fd5-9755-33ae01683dc1"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "K1400/1670",
                    MainStrength = 1400e6
                },
                new ReinforcementMaterialEntity(new Guid("93c48a27-ab37-4bd2-aeb8-2a7247e74a1b"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "K1500/1770",
                    MainStrength = 1500e6
                },
                new ReinforcementMaterialEntity(new Guid("6e0df35e-4839-4cf1-9182-c7ad7f81a548"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "K1600/1860",
                    MainStrength = 1600e6
                },
                new ReinforcementMaterialEntity(new Guid("29d7ef1b-bd30-471e-af0e-8b419eb9f043"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "K1700/1960",
                    MainStrength = 1700e6
                },
                new ReinforcementMaterialEntity(new Guid("494b959f-0194-4f02-9dcf-ff313c5e352b"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "K1800/2060",
                    MainStrength = 1800e6
                },
                new ReinforcementMaterialEntity(new Guid("02031332-fe1e-456d-b339-143eb9ca8293"))
                {
                    CodeType = codeType,
                    Code = code,
                    Name = "K1900/2160",
                    MainStrength = 1900e6
                }
            };
            return range;
        }
    }
}
