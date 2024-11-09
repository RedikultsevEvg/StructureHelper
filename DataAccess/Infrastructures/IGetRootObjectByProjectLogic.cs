using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public interface IGetRootObjectByProjectLogic : IGetRootObjectLogic
    {
        IProject Project { get; set; }
    }
}
