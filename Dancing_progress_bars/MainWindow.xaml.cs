using System;
using System.Threading;
using System.Windows;

namespace Dancing_progress_bars;

public partial class MainWindow : Window
{
    public SynchronizationContext UiContext;

    public MainWindow()
    {
        InitializeComponent();

        // Положение окна при страте по центру экрана.
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        // Получим контекст синхронизации для текущего потока.
        UiContext = SynchronizationContext.Current;
    }

    // Метод, 
    private void StartCheckBox_Click1(object sender, RoutedEventArgs e)
    {
        // Создание делегата функции, в которой будет работать новый поток.
        ThreadStart methodThread = ThreadFunk;
        // Создание объекта потока.
        var thread = new Thread(methodThread);
        thread.IsBackground = true;
        // Старт потока.
        thread.Start();
    }

    private void ThreadFunk()
    {
        try
        {
            Application.Current.Dispatcher.Invoke(() => ProgressBar1.Minimum = 0);
            Application.Current.Dispatcher.Invoke(() => ProgressBar1.Maximum = 100);
            Application.Current.Dispatcher.Invoke(() => ProgressBar1.Value = 0);

            while (StartCheckBox1.IsChecked == true && CheckBoxStop1.IsChecked == false)
            {
                // Получаем случайное значение.
                var randomValue = GetRandomValue();
                // Обновляем значение ProgressBar1.Value с задержкой в 50 мс.
                UiContext.Send(d => ProgressBar1.Value = randomValue, null);
                Thread.Sleep(50);
            }

            UiContext.Send(d =>
            {
                // Проверяем состояние чекбокса.
                if (StartCheckBox1.IsChecked == true)
                {
                    StartLabel1.Content = "Остановить 1-й поток";
                    StackPanelStop1.IsEnabled = true;
                }
                else
                {
                    StartLabel1.Content = "Запустить 1-й поток";
                    CheckBoxStop1.IsChecked = false;
                    LabelStop1.Content = "Приостановить 1-й поток";
                    ProgressBar1.Value = 0;
                    StackPanelStop1.IsEnabled = false;
                }
            }, null);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    // Метод возвращает случайное значение от 0 до 100 включительно.
    private int GetRandomValue()
    {
        var random = new Random();
        return random.Next(0, 101);
    }
}