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
        readonly ReadOnlyCollection<TreeViewItemViewModel> _children;
        readonly TreeViewItemViewModel _parent;
        readonly IOneVariableFunction _functions;

        bool _isExpanded;
        bool _isSelected;

        public TreeViewItemViewModel(IOneVariableFunction function) : this(function, null)
        {
        }
        private TreeViewItemViewModel(IOneVariableFunction function, TreeViewItemViewModel parent)
        {
            _functions = function;
            _parent = parent;
            _children = new ReadOnlyCollection<TreeViewItemViewModel>
                (
                _functions.Functions
                .Select(x => new TreeViewItemViewModel(x, this))
                .ToList<TreeViewItemViewModel>()
                );
        }
        public ReadOnlyCollection<TreeViewItemViewModel> Children
        {
            get { return _children; }
        }

        public string Name
        {
            get { return _functions.Name; }
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
