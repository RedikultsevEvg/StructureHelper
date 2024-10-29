using FieldVisualizer.ViewModels;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.TreeGraph
{
    public class TreeViewItemViewModel : ViewModelBase
    {
        readonly ObservableCollection<TreeViewItemViewModel> _children;
        readonly TreeViewItemViewModel _parent;
        readonly IOneVariableFunction _function;
        readonly TreeGraphViewModel _treeGraphViewModel;

        bool _isExpanded;
        bool _isSelected;

        public TreeViewItemViewModel(IOneVariableFunction function, TreeGraphViewModel treeGraphViewModel) : this(function, null, treeGraphViewModel)
        {
        }
        private TreeViewItemViewModel(IOneVariableFunction function, TreeViewItemViewModel parent, TreeGraphViewModel treeGraphViewModel)
        {
            _function = function;
            _parent = parent;
            _treeGraphViewModel = treeGraphViewModel;
            _children = new ObservableCollection<TreeViewItemViewModel>
                (
                _function.Functions
                .Select(x => new TreeViewItemViewModel(x, this, treeGraphViewModel))
                .ToList<TreeViewItemViewModel>()
                );
        }
        public IOneVariableFunction Function
        {
            get { return _function; }
        }
        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return _children; }
        }
        public string Name
        {
            get { return _function.Name; }
        }
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                }
                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    _treeGraphViewModel.DrawGraph();
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public TreeViewItemViewModel Parent
        {
            get { return _parent; }
        }

    }
}
