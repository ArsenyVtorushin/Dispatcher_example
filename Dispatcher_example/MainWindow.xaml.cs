using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Dispatcher_example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartNotepad_Click(object sender, RoutedEventArgs e)
        {
            Process process = Process.Start("notepad.exe");
            if (process != null)
            {
                try
                {
                    process.CloseMainWindow();
                    process.WaitForExit(3000);
                    if (!process.HasExited)
                        process.Kill();
                }
                catch (Exception ex)
                {
                    stateTextBlock.Text = $"Ошибка при закрытии блокнота: {ex.Message}";
                }
            }
        }

        private void ReloadTextBlock_Click(object sender, RoutedEventArgs e)
        {
            Thread backgroundThread = new Thread
        }
    }
}