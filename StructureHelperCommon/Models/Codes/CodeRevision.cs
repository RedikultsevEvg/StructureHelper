using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Codes
{
    public class CodeRevision : ICodeRevision
    {
        public Guid Id { get; }
        public ICodeEntity CodeEntity { get; set; }
        public string RevisionNumber { get; set; }
        public DateOnly PublicationDate { get; set; }
        public RevisionStatus RevisionStatus { get; set; }
        public string FullName => $"{CodeEntity.FullName} Rev. {RevisionNumber} ({PublicationDate})";

        public CodeRevision(Guid id)
        {
            Id = id;
        }

    }
}
