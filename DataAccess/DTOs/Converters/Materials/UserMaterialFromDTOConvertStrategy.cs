using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class UserMaterialFromDTOConvertStrategy : ConvertStrategy<UserMaterial, UserMaterialDTO>
    {
        private IUpdateStrategy<IUserMaterial> updateStrategy;
        private IUpdateStrategy<IUserMaterial> UpdateStrategy => updateStrategy ??= new UserMaterialUpdateStrategy() { UpdateChildren = false };

        public override UserMaterial GetNewItem(UserMaterialDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
