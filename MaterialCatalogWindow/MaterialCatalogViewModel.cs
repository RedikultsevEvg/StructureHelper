using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using StructureHelper.Annotations;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace StructureHelper
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MaterialCatalogViewModel : INotifyPropertyChanged
    {
        private MaterialCatalogModel materialCatalogModel;
        private MaterialCatalogView materialCatalogView;

        [JsonProperty]
        public ObservableCollection<MaterialDefinitionBase> ConcreteDefinitions { get; set; }
        [JsonProperty]
        public ObservableCollection<MaterialDefinitionBase> RebarDefinitions { get; set; }
        public ICommand AddMaterial { get; }
        public ICommand SaveCatalog { get; }
        public ICommand LoadCatalog { get; }
        public ICommand SelectMaterial { get; }

        private MaterialDefinitionBase selectedMaterial;
        public MaterialDefinitionBase SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                selectedMaterial = value;
                OnPropertyChanged();
            }
        }

        private bool isMaterialCanBeSelected;

        public bool IsMaterialCanBeSelected
        {
            get => isMaterialCanBeSelected;
            set
            {
                isMaterialCanBeSelected = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectMaterialButtonVisibility));
            }
        }

        public Visibility SelectMaterialButtonVisibility => IsMaterialCanBeSelected ? Visibility.Visible : Visibility.Hidden;
        public MaterialCatalogViewModel() { }

        public MaterialCatalogViewModel(MaterialCatalogModel materialCatalogModel, MaterialCatalogView materialCatalogView, bool isMaterialCanBeSelected, PrimitiveDefinition primitive = null)
        {
            this.materialCatalogModel = materialCatalogModel;
            this.materialCatalogView = materialCatalogView;
            IsMaterialCanBeSelected = isMaterialCanBeSelected;

            ConcreteDefinitions = new ObservableCollection<MaterialDefinitionBase>(materialCatalogModel.ConcreteDefinitions);
            RebarDefinitions = new ObservableCollection<MaterialDefinitionBase>(materialCatalogModel.RebarDefinitions);

            AddMaterial = new RelayCommand(o =>
            {
                AddMaterialView addMaterialView = new AddMaterialView(this.materialCatalogModel, this);
                addMaterialView.ShowDialog();
                OnPropertyChanged(nameof(ConcreteDefinitions));
                OnPropertyChanged(nameof(RebarDefinitions));
            });
            SaveCatalog = new RelayCommand(o =>
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.InitialDirectory = @"C:\";
                saveDialog.Filter = "json files (*.json)|*.json|All files(*.*)|*.*";
                string path = null;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    path = saveDialog.FileName;
                    File.WriteAllText(path, json);
                }
            });
            LoadCatalog = new RelayCommand(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "json files (*.json)|*.json|All files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var path = openFileDialog.FileName;
                    var content = File.ReadAllText(path);
                    var vm = JsonConvert.DeserializeObject<MaterialCatalogViewModel>(content);
                    ConcreteDefinitions = vm.ConcreteDefinitions;
                    OnPropertyChanged(nameof(ConcreteDefinitions));
                    RebarDefinitions = vm.RebarDefinitions;
                    OnPropertyChanged(nameof(RebarDefinitions));
                }
            });
            SelectMaterial = new RelayCommand(o =>
            {
                if (primitive != null)
                    primitive.Material = SelectedMaterial;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
