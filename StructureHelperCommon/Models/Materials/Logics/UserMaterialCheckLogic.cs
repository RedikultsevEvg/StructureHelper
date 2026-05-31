using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;

namespace StructureHelperCommon.Models.Materials.Logics
{
    public class UserMaterialCheckLogic : CheckEntityLogic<IUserMaterial>
    {
        public override bool Check()
        {
            bool result = true;
            if (Entity.FilePath == string.Empty)
            {
                TraceMessage("File path is empty");
                result = false;
            }
            if (File.Exists(Entity.FilePath) == false)
            {
                TraceMessage($"File {Entity.FilePath} does not exsist");
                result = false;
            }
            return result;
        }
    }
}
