using System.Windows.Input;

namespace StructureHelper.Infrastructure
{
    public class DoubleClickEventTrigger : EventTriggerBase<MouseButtonEventArgs>
    {
        public DoubleClickEventTrigger() : base(args => args.ClickCount == 2) { }
    }
}
