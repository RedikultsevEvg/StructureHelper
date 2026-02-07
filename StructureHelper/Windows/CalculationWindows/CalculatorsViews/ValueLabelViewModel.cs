using StructureHelper.Infrastructure;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class ValueLabelViewModel : ViewModelBase
    {
		private double labelSize;

		public double LabelSize
		{
			get { return labelSize; }
			set
			{
				double oldValue = labelSize;
				try
				{
					labelSize = value;
				}
				catch
				{
					labelSize = oldValue;
				}
				OnPropertyChanged(nameof(LabelSize));
			}
		}

	}
}
