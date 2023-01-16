using FieldVisualizer.ViewModels;
using StructureHelperCommon.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class SecondOrderViewModel : ViewModelBase
    {
        ICompressedMember member;

        public bool Buckling
        {
            get => member.Buckling;
            set
            {
                member.Buckling = value;
                OnPropertyChanged(nameof(Buckling));
            }
        }

        public double GeometryLength
        {
            get => member.GeometryLength;
            set
            {
                member.GeometryLength = value;
                OnPropertyChanged(nameof(GeometryLength));
            }
        }

        public double LengthFactorX
        {
            get => member.LengthFactorX;
            set
            {
                member.GeometryLength = value;
                OnPropertyChanged(nameof(LengthFactorX));
            }
        }

        public double LengthFactorY
        {
            get => member.LengthFactorY;
            set
            {
                member.GeometryLength = value;
                OnPropertyChanged(nameof(LengthFactorY));
            }
        }

        public SecondOrderViewModel(ICompressedMember compressedMember)
        {
            member = compressedMember;
        }
    }
}
