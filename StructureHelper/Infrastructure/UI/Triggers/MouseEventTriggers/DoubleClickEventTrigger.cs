using System.Windows.Input;

namespace StructureHelper.Infrastructure.UI.Triggers.MouseEventTriggers
{
    public class DoubleClickEventTrigger : EventTriggerBase<MouseButtonEventArgs>
    {
        public DoubleClickEventTrigger() : base(args => args.ClickCount == 2) { }
    }
}
