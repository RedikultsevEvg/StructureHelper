using System.Windows.Input;

namespace StructureHelper.Infrastructure
{
    public class MouseWheelDownEventTrigger : EventTriggerBase<MouseWheelEventArgs>
    {
        public MouseWheelDownEventTrigger() : base(args => args.Delta > 0) { }
    }
}
