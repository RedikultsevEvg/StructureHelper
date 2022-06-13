using System.Windows.Input;

namespace StructureHelper.Infrastructure
{
    public class MouseWheelUpEventTrigger : EventTriggerBase<MouseWheelEventArgs>
    {
        public MouseWheelUpEventTrigger() : base(args => args.Delta < 0) { }
    }
}