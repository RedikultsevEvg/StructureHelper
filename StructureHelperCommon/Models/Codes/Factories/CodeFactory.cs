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
        public static (List<ICodeEntity> codes, List<ICodeRevision> codeRevisions) GetCodeEntities()
        {
            List<ICodeEntity> codes = new List<ICodeEntity>();
            List<ICodeRevision> codeRevisions = new();
            (List<ICodeEntity> codes, List<ICodeRevision> codeRevisions) russianCodes = GetRussianCodes();
            codes.AddRange(russianCodes.codes);
            codeRevisions.AddRange(russianCodes.codeRevisions);
            codes.AddRange(GetEuropeanCodes());
            return (codes, codeRevisions);
        }

        private static (List<ICodeEntity> codes, List<ICodeRevision> codeRevisions) GetRussianCodes()
        {
            const NatSystems natSystem = NatSystems.RU;
            List<ICodeEntity> codeEntities = new();
            List<ICodeRevision> codeRevisions = new();
            ICodeEntity codeEntity;
            ICodeRevision codeRevision;
            var code = GetSP63_13330_2018(natSystem);
            codeEntities.Add(code.code);
            codeRevisions.AddRange(code.codeRevisions);
            codeEntity = new CodeEntity(new Guid("1a717049-cee7-40e0-923c-7a32a573a303"), natSystem)
            {
                Name = "GOST 26633-2015",
                FullName = "Heavy-weight and sand concretes. Specifications"
            };
            codeEntities.Add(codeEntity);
            codeEntity = new CodeEntity(new Guid("c7c0f60f-2c82-45d1-8786-4c340fb5fb98"), natSystem)
            {
                Name = "GOST 34028-2016",
                FullName = "Reinforcing rolled products for reinforced concrete constructions. Specifications"
            };
            codeEntities.Add(codeEntity);
            codeEntity = new CodeEntity(new Guid("d934763d-4cb4-4923-ad15-2e78b0fe3b37"), natSystem)
            {
                Name = "GOST 53772-2010",
                FullName = "Reinforced steel low-relaxation 7-wire strands. Specifications"
            };
            codeEntities.Add(codeEntity);
            return (codeEntities, codeRevisions);
        }

        private static (ICodeEntity code, List<ICodeRevision> codeRevisions) GetSP63_13330_2018(NatSystems natSystem)
        {
            ICodeEntity codeEntity;
            ICodeRevision codeRevision;
            List<ICodeRevision> codeRevisions = new();
            codeEntity = new CodeEntity(new Guid("d4ab402a-ce2f-46db-8b3b-a5a66fb384e1"), natSystem)
            {
                Name = "SP 63.13330.2018",
                FullName = "Plain concrete and reinforced concrete structures"
            };
            codeRevision = new CodeRevision(new Guid("1c6047df-cc40-413e-8ed1-c0043182fe0a"))
            {
                CodeEntity = codeEntity,
                RevisionNumber = "0",
                PublicationDate = new DateOnly(2018, 1, 1),
                RevisionStatus = RevisionStatus.Withdrawn
            };
            codeRevisions.Add(codeRevision);
            codeRevision = new CodeRevision(new Guid("f9f66a43-e50d-40bf-8a82-0359b54a3ab0"))
            {
                CodeEntity = codeEntity,
                RevisionNumber = "1",
                PublicationDate = new DateOnly(2019, 1, 1),
                RevisionStatus = RevisionStatus.Withdrawn
            };
            codeRevisions.Add(codeRevision);
            codeRevision = new CodeRevision(new Guid("010325ae-5182-4d8e-9b30-7a3ec4860745"))
            {
                CodeEntity = codeEntity,
                RevisionNumber = "2",
                PublicationDate = new DateOnly(2020, 1, 1),
                RevisionStatus = RevisionStatus.Withdrawn
            };
            codeRevisions.Add(codeRevision);
            codeRevision = new CodeRevision(new Guid("17b6fb62-97b3-4c13-bf38-b91ea16af819"))
            {
                CodeEntity = codeEntity,
                RevisionNumber = "3",
                PublicationDate = new DateOnly(2021, 1, 1),
                RevisionStatus = RevisionStatus.Official
            };
            codeRevisions.Add(codeRevision);
            return (codeEntity, codeRevisions);
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
