﻿using System;
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

namespace StructureHelper.Windows.MainGraph
{
    /// <summary>
    /// Interaction logic for AddTable.xaml
    /// </summary>
    public partial class TableView : Window
    {
        private TableViewModel viewModel;

        public TableView(TableViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }
        /*public AddTable() : this(new AddTableViewModel())
        {
        }*/
        public TableView()
        {
            InitializeComponent();
        }

    }
}
