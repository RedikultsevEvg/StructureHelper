using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using StructureHelper.Infrastructure;

namespace StructureHelper
{
    public class AddMaterialViewModel : ViewModelBase
    {
        private MaterialCatalogModel model;
        private AddMaterialView view;
        private MaterialCatalogViewModel materialCatalogViewModel;
        public ObservableCollection<NamedList<MaterialDefinitionBase>> Materials { get; set; }
        private NamedList<MaterialDefinitionBase> materialCollection;
        public NamedList<MaterialDefinitionBase> MaterialCollection
        {
            get => materialCollection;
            set
            {
                OnPropertyChanged(value, materialCollection);
                OnPropertyChanged(nameof(IsNotConcrete));
                OnPropertyChanged(nameof(RowHeight));
            }
        }
        public bool IsNotConcrete => MaterialCollection.Name != "Бетон";

        private string materialClass;
        private double youngModulus, compressiveStrengthCoef, tensileStrengthCoef, materialCoefInCompress, materialCoefInTension;

        public string MaterialClass
        {
            get => materialClass;
            set => OnPropertyChanged(value, ref materialClass);
        }

        public double YoungModulus
        {
            get => youngModulus;
            set => OnPropertyChanged(value, ref youngModulus);
        }
        public double CompressiveStrengthCoef
        {
            get => compressiveStrengthCoef;
            set => OnPropertyChanged(value, ref compressiveStrengthCoef);
        }
        public double TensileStrengthCoef
        {
            get => tensileStrengthCoef;
            set => OnPropertyChanged(value, ref tensileStrengthCoef);
        }
        public double MaterialCoefInCompress
        {
            get => materialCoefInCompress;
            set => OnPropertyChanged(value, ref materialCoefInCompress);
        }
        public double MaterialCoefInTension
        {
            get => materialCoefInTension;
            set => OnPropertyChanged(value, ref materialCoefInTension);
        }
        
        public int RowHeight => IsNotConcrete ? 40 : 0;
        public ICommand AddMaterial { get; }
        public AddMaterialViewModel() { }
        public AddMaterialViewModel(MaterialCatalogModel model, AddMaterialView view, MaterialCatalogViewModel materialCatalogViewModel)
        {
            this.model = model;
            this.view = view;
            this.materialCatalogViewModel = materialCatalogViewModel;
            Materials = new ObservableCollection<NamedList<MaterialDefinitionBase>>(model.Materials);
            MaterialCollection = Materials.First();

            AddMaterial = new RelayCommand(o =>
            {
                if (MaterialCollection.Name == "Бетон")
                    this.materialCatalogViewModel.ConcreteDefinitions.Add(new ConcreteDefinition(MaterialClass, 0, CompressiveStrengthCoef, TensileStrengthCoef, MaterialCoefInCompress, MaterialCoefInTension));
                if (MaterialCollection.Name == "Арматура")
                    this.materialCatalogViewModel.RebarDefinitions.Add(new RebarDefinition(MaterialClass, YoungModulus, CompressiveStrengthCoef, TensileStrengthCoef, MaterialCoefInCompress, MaterialCoefInTension));
            });
        }
    }
}
