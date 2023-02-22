using System;
using System.Threading;
using System.Windows;

namespace Dancing_progress_bars;

public partial class MainWindow : Window
{
    private readonly ManualResetEvent event_for_stop = new(false);
    private readonly ManualResetEvent event_for_suspend = new(true);

    private readonly SynchronizationContext UiContext;

    public MainWindow()
    {
        InitializeComponent();

        // Положение окна при страте по центру экрана.
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        // Получим контекст синхронизации для текущего потока.
        UiContext = SynchronizationContext.Current;
    }

    private void ThreadFunk()
    {
        try
        {
            UiContext.Send(d => ProgressBar1.Minimum = 0, null);
            UiContext.Send(d => ProgressBar1.Maximum = 100, null);
            UiContext.Send(d => ProgressBar1.Value = 0, null);

            while (true)
            {
                event_for_suspend.WaitOne(); // Если сигналит усыпляет.
                Thread.Sleep(250);
                UiContext.Send(d => ProgressBar1.Value = GetRandomValue(), null);
            }
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


    #region 1 поток (1 слайдер)

    // Пуск 1 фонового потока для 1 слайдера.
    private void StartCheckBox1_Checked(object sender, RoutedEventArgs e)
    {
        // Создаем и запускаем поток.
        var th1 = new Thread(ThreadFunk);
        th1.Name = "Thread1";
        th1.IsBackground = true;
        th1.Start();
        UiContext.Send(d => StartLabel1.Content = "Остановить 1-й поток", null);
        UiContext.Send(d => StackPanelStop1.IsEnabled = true, null);
    }

    private void StartCheckBox1_Unchecked(object sender, RoutedEventArgs e)
    {
        UiContext.Send(d => StartLabel1.Content = "Запустить 1-й поток", null);
        UiContext.Send(d => StackPanelStop1.IsEnabled = false, null);
        UiContext.Send(d => CheckBoxStop1.IsChecked = false, null);
        
    }

    private void CheckBoxStop1_Checked(object sender, RoutedEventArgs e)
    {
        UiContext.Send(d => LabelStop1.Content = "Возобновить 1-й поток", null);
        event_for_suspend.Reset();
    }

    private void CheckBoxStop1_Unchecked(object sender, RoutedEventArgs e)
    {
        UiContext.Send(d => LabelStop1.Content = "Приостановить 1-й поток", null);
        event_for_suspend.Set();
    }

    #endregion


    #region 2 поток (2 слайдер)

    #endregion


    #region 3 поток (3 слайдер)

    #endregion
}

// Запустить 3 фоновых потока.
// 20:24:35 событие с ручным сбросом