using StructureHelper.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainWindow
{
   public class CrossSectionVisualPropertyVM : ViewModelBase
   {
      private double axisLineThickness;
      private double gridLineThickness;
      private double gridSize;
      private double workPlainWidth;
      private double workPlainHeight;

      /// <summary>
      /// Thickness of x-, and y- axis line
      /// </summary>
      public double AxisLineThickness
      {
         get => axisLineThickness; set
         {
            axisLineThickness = value;
            OnPropertyChanged(nameof(AxisLineThickness));
         }
      }
      /// <summary>
      /// Thickness of lines of coordinate mesh
      /// </summary>
      public double GridLineThickness
      {
         get => gridLineThickness; set
         {
            gridLineThickness = value;
            OnPropertyChanged(nameof(GridLineThickness));
         }
      }
      /// <summary>
      /// Size of coordinate mesh
      /// </summary>
      public double GridSize
      {
         get => gridSize; set
         {
            gridSize = value;
            OnPropertyChanged(nameof(GridSize));
         }
      }
      /// <summary>
      /// Width of work plane
      /// </summary>
      public double WorkPlainWidth
      {
         get => workPlainWidth; set
         {
            workPlainWidth = value;
            OnPropertyChanged(nameof(WorkPlainWidth));
         }
      }
      /// <summary>
      /// Height of work plane
      /// </summary>
      public double WorkPlainHeight
      {
         get => workPlainHeight; set
         {
            workPlainHeight = value;
            OnPropertyChanged(nameof(WorkPlainHeight));
         }
      }

      public CrossSectionVisualPropertyVM()
      {
         AxisLineThickness = 2d;
         GridLineThickness = 0.25d;
         GridSize = 0.05d;
         WorkPlainWidth = 1.2d;
         WorkPlainHeight = 1.2d;
      }
   }
}
