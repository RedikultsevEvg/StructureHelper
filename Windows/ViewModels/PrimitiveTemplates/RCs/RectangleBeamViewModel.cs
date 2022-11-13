using StructureHelper.Infrastructure;
using StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Templates.RCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.PrimitiveTemplates.RCs
{
    internal class RectangleBeamViewModel : ViewModelBase, IDataErrorInfo
    {
		public IRectangleBeamTemplate Model;

		private Rectangle rectangle => (Model.Shape as Rectangle);

		public Window ParentWindow { get; set; }

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public double Width
		{
			get { return rectangle.Width; }
			set { rectangle.Width = value; }
		}
		public double Height
		{
			get { return rectangle.Height; }
			set { rectangle.Height = value; }
		}
		public double CoverGap
		{
			get { return Model.CoverGap; }
			set { Model.CoverGap = value; }
		}
        public double TopDiameter
        {
            get { return Model.TopDiameter; }
            set { Model.TopDiameter = value; }
        }
        public double BottomDiameter
		{
			get { return Model.BottomDiameter; }
			set { Model.BottomDiameter = value; }
		}
		public int WidthCount
		{
			get { return Model.WidthCount; }
			set { Model.WidthCount = value; }
		}
		public int HeightCount
		{
			get { return Model.HeightCount; }
			set { Model.HeightCount = value; }
		}

		public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == nameof(Width)
                    ||
                    columnName == nameof(Height))
                {
                    if (this.Width <= 0 || this.Height <= 0)
                    {
                        error = "Width and Height of rectangle must be greater than zero";
                    }
                }
                else if (columnName == nameof(TopDiameter) || columnName == nameof(BottomDiameter))
                {
                    if (CoverGap <0 )
                    {
                        error = "Diameter must be grater than zero";
                    }
                }
                return error;
            }
        }

        public RectangleBeamViewModel(IRectangleBeamTemplate template)
		{
			Model = template;

            OkCommand = new RelayCommand(o => OkAction());
            CancelCommand = new RelayCommand(o => CancelAction());
        }


        private void CancelAction()
        {
			ParentWindow.DialogResult = false;
			ParentWindow.Close();
        }

        private void OkAction()
        {
            ParentWindow.DialogResult = true;
            ParentWindow.Close();
        }

    }
}
