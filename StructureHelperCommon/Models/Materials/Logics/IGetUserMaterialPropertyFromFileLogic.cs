using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface IGetUserMaterialPropertyFromFileLogic : ILogic
    {
        string FilePath { get; set; }

        IUserMaterialProperty GetUserMaterialProperty();
    }
}
