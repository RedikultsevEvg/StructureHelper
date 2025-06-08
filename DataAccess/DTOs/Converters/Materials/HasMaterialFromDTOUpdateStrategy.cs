using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2024 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    /// <summary>
    /// Creates copies of materials from source and adds them into target material list
    /// </summary>
    public class HasMaterialFromDTOUpdateStrategy : IUpdateStrategy<IHasHeadMaterials>
    {
        private readonly IConvertStrategy<IHeadMaterial, IHeadMaterial> convertStrategy;

        public HasMaterialFromDTOUpdateStrategy(IConvertStrategy<IHeadMaterial, IHeadMaterial> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public void Update(IHasHeadMaterials targetObject, IHasHeadMaterials sourceObject)
        {
            targetObject.HeadMaterials.Clear();
            foreach (var item in sourceObject.HeadMaterials)
            {
                var newItem = convertStrategy.Convert(item);
                targetObject.HeadMaterials.Add(newItem);
            }
        }
    }
}
