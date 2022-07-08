using System;
using System.Windows;
using EventTrigger = System.Windows.Interactivity.EventTrigger;

namespace StructureHelper.Infrastructure
{
    public class EventTriggerBase<T> : EventTrigger where T : RoutedEventArgs
    {
        readonly Predicate<T> eventTriggerPredicate;

        public EventTriggerBase(Predicate<T> eventTriggerPredicate)
        {
            this.eventTriggerPredicate = eventTriggerPredicate;
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            if (eventArgs is T e && eventTriggerPredicate(e))
            {
                base.OnEvent(eventArgs);
                e.Handled = true;
            }
        }
    }
}