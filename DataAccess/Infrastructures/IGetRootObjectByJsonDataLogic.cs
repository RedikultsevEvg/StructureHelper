using DataAccess.DTOs;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public interface IGetRootObjectByJsonDataLogic : IGetRootObjectLogic
    {
        string JsonData { get; set; }
    }
}
