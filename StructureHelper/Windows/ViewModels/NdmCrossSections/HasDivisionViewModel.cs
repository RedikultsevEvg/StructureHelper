using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class HasDivisionViewModel : ViewModelBase, IDivisionSize
    {
        private IDivisionSize primitive;
        public Guid Id { get; }

        public double NdmMaxSize
        { 
            get { return primitive.NdmMaxSize; }
            set
            {
                primitive.NdmMaxSize = value;
                OnPropertyChanged(nameof(NdmMaxSize));
            }
        }
        public int NdmMinDivision
        {
            get { return primitive.NdmMinDivision; }
            set
            {
                primitive.NdmMinDivision = value;
                OnPropertyChanged(nameof(NdmMinDivision));
            }
        }
        public bool ClearUnderlying
        {
            get { return primitive.ClearUnderlying; }
            set
            {
                primitive.ClearUnderlying = value;
                OnPropertyChanged(nameof(ClearUnderlying));
            }
        }


        public HasDivisionViewModel(IDivisionSize primitive)
        {
            this.primitive = primitive;
        }

        public bool IsPointInside(IPoint2D point)
        {
            //not required
            throw new NotImplementedException();
        }
    }
}
