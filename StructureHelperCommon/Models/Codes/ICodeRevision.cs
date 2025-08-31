using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Codes
{
    public enum RevisionStatus
    {
        Draft,
        Official,
        Withdrawn
    }

    public interface ICodeRevision : ISaveable
    {
        ICodeEntity CodeEntity { get; set; }
        string RevisionNumber { get; set; }
        string FullName { get; }
        DateOnly PublicationDate { get; set; }
        RevisionStatus RevisionStatus { get; set; }
    }
}
