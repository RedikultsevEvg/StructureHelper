using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    internal class StirrupGroupToDTOConvertStrategy : ConvertStrategy<StirrupGroupDTO, IStirrupGroup>
    {
        
        public override StirrupGroupDTO GetNewItem(IStirrupGroup source)
        {
            throw new NotImplementedException();
        }
    }
}
