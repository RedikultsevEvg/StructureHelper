using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using System.Collections.Generic;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearLoadsViewModel : SelectItemVM<IBeamSpanLoad>
    {
        private IUpdateStrategy<IBeamSpanLoad> updateStrategy;
        public BeamShearLoadsViewModel(List<IBeamSpanLoad> collection) : base(collection)
        {
        }
        public override void AddMethod(object parameter)
        {
            LoadTypes typedParameter = (LoadTypes)parameter;
            if (typedParameter == LoadTypes.DistributetLoad)
            {
                NewItem = BeamShearLoadFactory.GetBeamShearLoad(ShearLoadTypes.DistributedLoad);
            }
            else if (typedParameter == LoadTypes.ConcentratedForce)
            {
                NewItem = BeamShearLoadFactory.GetBeamShearLoad(ShearLoadTypes.ConcentratedForce);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(parameter));
            }
            base.AddMethod(parameter);
        }
        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            Window window;
            IBeamSpanLoad temporaryShearLoad = SelectedItem.Clone() as IBeamSpanLoad;
            if (SelectedItem is IDistributedLoad distributedLoad)
            {
                window = new DistributedLoadView(distributedLoad);
            }
            else if (SelectedItem is IConcentratedForce concentrated)
            {
                window = new ConcentratedForceView(concentrated);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
            window.ShowDialog();
            if (window.DialogResult != true)
            {
                updateStrategy ??= new BeamShearLoadUpdateStrategy();
                updateStrategy.Update(SelectedItem, temporaryShearLoad);
            }
            base.EditMethod(parameter);
        }
    }
}
