using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public interface IGetJsonSettingsLogic : ILogic
    {
        JsonSerializerSettings GetSettings();
    }
}
