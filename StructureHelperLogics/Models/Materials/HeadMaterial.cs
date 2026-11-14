using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using System.ComponentModel;
using System.Windows.Media;

namespace StructureHelper.Models.Materials
{
    public class HeadMaterial : IHeadMaterial, INotifyPropertyChanged
    {
        private HeadMaterialUpdateStrategy updateStrategy = new HeadMaterialUpdateStrategy();
        private Color color;

        public Guid Id { get; }
        public string Name { get; set; }
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
            }
        }
        public IHelperMaterial HelperMaterial {get; set;}


        public HeadMaterial(Guid id)
        {
            Id = id;
            Color = ColorProcessor.GetRandomColor();
        }

        public HeadMaterial() : this(Guid.NewGuid())
        {        
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return HelperMaterial.GetLoaderMaterial(limitState, calcTerm);
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return HelperMaterial.GetCrackedLoaderMaterial(limitState, calcTerm);
        }


        public IHeadMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var newItem = new HeadMaterial(id);
            updateStrategy.Update(newItem, this);
            newItem.HelperMaterial = HelperMaterial.GetClone(CloneMode.NewEntity, CloneDepth.Deep);
            return newItem;
        }
    }
}
