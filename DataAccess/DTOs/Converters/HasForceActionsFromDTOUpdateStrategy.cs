using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class HasForceActionsFromDTOUpdateStrategy : IUpdateStrategy<IHasForceActions>
    {
        private readonly IConvertStrategy<IForceAction, IForceAction> convertStrategy;

        public HasForceActionsFromDTOUpdateStrategy(IConvertStrategy<IForceAction, IForceAction> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public void Update(IHasForceActions targetObject, IHasForceActions sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ForceActions.Clear();
            foreach (var item in sourceObject.ForceActions)
            {
                var newItem = convertStrategy.Convert(item);
                targetObject.ForceActions.Add(newItem);
            }
        }
    }
}
