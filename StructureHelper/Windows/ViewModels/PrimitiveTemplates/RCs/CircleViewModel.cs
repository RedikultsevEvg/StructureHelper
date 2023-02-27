using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.PrimitiveTemplates.RCs
{
    internal class CircleViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        public ICircleTemplate Model;

        public double Diameter
        {
            get { return Model.Shape.Diameter; }
            set { Model.Shape.Diameter = value; }
        }
        public double CoverGap
        {
            get { return Model.CoverGap; }
            set { Model.CoverGap = value; }
        }
        public double BarDiameter
        {
            get { return Model.BarDiameter; }
            set { Model.BarDiameter = value; }
        }
        public int BarCount
        {
            get { return Model.BarCount; }
            set { Model.BarCount = value; }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == nameof(Diameter))
                {
                    if (this.Diameter <= 0)
                    {
                        error = "Diameter of section must be greater than zero";
                    }
                }
                else if (columnName == nameof(BarDiameter))
                {
                    if (BarDiameter < 0)
                    {
                        error = "Diameter must be greater than zero";
                    }
                }
                else if (columnName == nameof(CoverGap))
                {
                    if (CoverGap > Diameter / 2)
                    {
                        error = "Cover gap is too big";
                    }
                }
                return error;
            }
        }
        public CircleViewModel(ICircleTemplate model)
        {
            Model = model;
        }
    }
}
