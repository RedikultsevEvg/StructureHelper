using System.Windows.Input;

namespace StructureHelper.Infrastructure.UI.Triggers.MouseEventTriggers
{
    public class MouseWheelUpEventTrigger : EventTriggerBase<MouseWheelEventArgs>
    {
        public MouseWheelUpEventTrigger() : base(args => args.Delta < 0) { }
    }
}