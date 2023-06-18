using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Models.Materials
{
    public class HeadMaterial : IHeadMaterial, INotifyPropertyChanged
    {
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

        public object Clone()
        {
            IHeadMaterial material = new HeadMaterial
            {
                Name = Name,
                Color = Color,
                HelperMaterial = HelperMaterial.Clone() as IHelperMaterial
            };
            return material;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
