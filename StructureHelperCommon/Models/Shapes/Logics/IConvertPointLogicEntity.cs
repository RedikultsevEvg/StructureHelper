using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    public interface IConvertPointLogicEntity : ISaveable
    {
        string Name { get; set; }
        IConvert2DPointTo3DPointLogic ConvertLogic { get;}
    }
}
