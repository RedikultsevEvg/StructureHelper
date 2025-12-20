using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class HeadMaterialViewModel : HeadMaterialBaseViewModel
    {
        private readonly HelperMaterialViewModel helperMaterialViewModel;

        public HeadMaterialViewModel(IHeadMaterial headMaterial) : base(headMaterial)
        {
            var helperMaterial = headMaterial.HelperMaterial;
            if (helperMaterial is IConcreteLibMaterial concreteMaterial)
            {
                HelperMaterialViewModel = new ConcreteViewModel(concreteMaterial);
            }
            else if (helperMaterial is IReinforcementLibMaterial reinforcementMaterial)
            {
                HelperMaterialViewModel = new LibMaterialViewModel<IReinforcementMaterialEntity>(reinforcementMaterial);
            }
            else if (helperMaterial is IElasticMaterial elasticMaterial)
            {
                if (helperMaterial is IFRMaterial fRMaterial)
                {
                    HelperMaterialViewModel = new FRViewModel(fRMaterial);
                }
                else
                {
                    HelperMaterialViewModel = new ElasticViewModel(elasticMaterial);
                }
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(IHelperMaterial)}, but was: {helperMaterial.GetType()}");
            }
        }
    }
}
