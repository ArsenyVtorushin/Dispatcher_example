using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
            Thread backgroundThread = new Thread(() =>
            {
                var notepadProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "C:\\Windows\\System32\\notepad.exe",
                        UseShellExecute = false
                    }
                };

                try
                {
                    notepadProcess.Start();

                    Dispatcher.Invoke(() =>
                    {
                        notepadState.Text = "Notepad работает...";
                    });

                    notepadProcess.WaitForExit(3000);
                    if (!notepadProcess.HasExited)
                        notepadProcess.Kill();

                    bool exited = notepadProcess.HasExited;

                    if (exited)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            notepadState.Text = "Notepad не работает.";
                        });
                    }
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 2) // ERROR_FILE_NOT_FOUND
                        Console.WriteLine("Файл не найден!");
                    else
                        Console.WriteLine($"Ошибка Windows: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Общая ошибка: {ex.Message}");
                }
                finally
                {
                    notepadProcess.Dispose();
                }
            });
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void StartCmd_Click(object sender, RoutedEventArgs e)
        {
            Thread backgroundThread = new Thread(() =>
            {
                var cmdProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "C:\\Windows\\System32\\cmd.exe",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                try
                {
                    cmdProcess.Start();
                    
                    using (StreamWriter input = cmdProcess.StandardInput)
                    {
                        input.WriteLine("dir");
                        input.WriteLine("exit");
                    }

                    string output = cmdProcess.StandardOutput.ReadToEnd();
                    string error = cmdProcess.StandardError.ReadToEnd();

                    cmdProcess.WaitForExit();

                    Dispatcher.Invoke(() =>
                    {
                        dir_textBlock.Text = output;

                        if (error != "")
                        {
                            dir_textBlock.Text = error;
                        }
                    });
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 2) // ERROR_FILE_NOT_FOUND
                        Console.WriteLine("Файл не найден!");
                    else
                        Console.WriteLine($"Ошибка Windows: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Общая ошибка: {ex.Message}");
                }
                finally
                {
                    cmdProcess.Dispose();
                }
            });
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
    }
}