﻿using StructureHelper.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.ProgressViews
{
    /// <summary>
    /// Логика взаимодействия для InterpolationProgressView.xaml
    /// </summary>
    public partial class ShowProgressView : Window
    {
        ShowProgressViewModel viewModel;
        public ShowProgressView(ShowProgressViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }
    }
}
