using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Infrastructure.Behaviors
{
    public static class FileDropBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(FileDropBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static void SetCommand(UIElement element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(UIElement element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                element.AllowDrop = true;

                element.PreviewDragOver += (sender, args) =>
                {
                    args.Effects = args.Data.GetDataPresent(DataFormats.FileDrop)
                        ? DragDropEffects.Copy
                        : DragDropEffects.None;
                    args.Handled = true;
                };

                element.Drop += (sender, args) =>
                {
                    if (args.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);
                        var command = GetCommand(element);
                        if (command?.CanExecute(files) == true)
                            command.Execute(files);
                    }
                };
            }
        }
    }
}
