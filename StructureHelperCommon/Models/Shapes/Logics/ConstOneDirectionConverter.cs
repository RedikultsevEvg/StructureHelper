using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    public class ConstOneDirectionConverter : IConvertPointLogicEntity
    {
        private ConstOneDirectionLogic convertLogic;

        public Guid Id { get; }
        public string Name { get; set; }
        public Directions Direction { get; private set; }
        public double ConstDirectionValue {get;set;}
        public string XAxisName { get; set; }
        public string YAxisName { get; set; }
        public string ConstAxisName { get; set; }
        public ForceTypes XForceType { get; set; }
        public ForceTypes YForceType { get; set; }
        public ForceTypes ZForceType { get; set; }

        public IConvert2DPointTo3DPointLogic ConvertLogic
        {
            get
            {
                convertLogic.ConstDirectionValue = ConstDirectionValue;
                return convertLogic;
            }
        }


        public ConstOneDirectionConverter(Directions direction, Guid guid)
        {
            Id = guid;
            convertLogic = new ConstOneDirectionLogic(direction, 0d);
        }
    }
}
