using System.Windows;
using System.Windows.Input;

namespace WpfArkanoid.Views
{
    /// <summary>
    /// Extends Window class with dependency properties relayed to commands.
    /// This makes classic keyboards events available without breaking the MVVM pattern.
    /// </summary>
    public class BaseWindow : Window
    {

        public ICommand KeyDownCommand
        {
            get { return (ICommand)GetValue(KeyDownCommandProperty); }
            set { SetValue(KeyDownCommandProperty, value); }
        }

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.Register("KeyDownCommand", typeof(ICommand), typeof(BaseWindow), new PropertyMetadata(null));

        public ICommand KeyUpCommand
        {
            get { return (ICommand)GetValue(KeyUpCommandProperty); }
            set { SetValue(KeyUpCommandProperty, value); }
        }

        public static readonly DependencyProperty KeyUpCommandProperty =
            DependencyProperty.Register("KeyUpCommand", typeof(ICommand), typeof(BaseWindow), new PropertyMetadata(null));


        public BaseWindow() {

        }

        private void BaseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownCommand != null)
                KeyDownCommand.Execute(e);
        }
        private void BaseWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyUpCommand != null)
                KeyUpCommand.Execute(e);
        }
    }
}
