using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class UserMaterialToDTOConvertStrategy : ConvertStrategy<UserMaterialDTO, IUserMaterial>
    {
        private IUpdateStrategy<IUserMaterial> updateStrategy;
        private IUpdateStrategy<IUserMaterial> UpdateStrategy => updateStrategy ??= new UserMaterialUpdateStrategy() { UpdateChildren = false};

        public override UserMaterialDTO GetNewItem(IUserMaterial source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
