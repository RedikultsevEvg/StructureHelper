﻿using StructureHelper.Windows.ViewModels.Graphs;
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

namespace StructureHelper.Windows.Graphs
{
    /// <summary>
    /// Логика взаимодействия для GraphView.xaml
    /// </summary>
    public partial class GraphView : Window
    {
        GraphViewModel vm;
        public GraphView(GraphViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            DataContext = vm;
        }
    }
}