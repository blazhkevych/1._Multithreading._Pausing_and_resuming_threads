using System;
using System.Threading;
using System.Windows;

namespace Dancing_progress_bars;

public partial class MainWindow : Window
{
    private readonly SynchronizationContext UiContext;

    public MainWindow()
    {
        InitializeComponent();

        // Положение окна при страте по центру экрана.
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        // Получим контекст синхронизации для текущего потока.
        UiContext = SynchronizationContext.Current;
    }

    // Метод возвращает случайное значение от 0 до 100 включительно.
    private int GetRandomValue()
    {
        var random = new Random();
        return random.Next(0, 101);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        event_for_stop1.Set();
    }

    #region 1 поток (1 слайдер)

    private readonly ManualResetEvent event_for_stop1 = new(false);
    private readonly ManualResetEvent event_for_suspend1 = new(true);

    private void ThreadFunk1()
    {
        try
        {
            UiContext.Send(d => ProgressBar1.Minimum = 0, null);
            UiContext.Send(d => ProgressBar1.Maximum = 100, null);
            UiContext.Send(d => ProgressBar1.Value = 0, null);

            while (true)
            {
                if (event_for_stop1.WaitOne(0)) break;

                event_for_suspend1.WaitOne(); // Если сигналит усыпляет.
                Thread.Sleep(250);
                UiContext.Send(d => ProgressBar1.Value = GetRandomValue(), null);
            }
            // Возможно тут стоит сбросить прогрессбар в valeu = 0;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    // Пуск 1 фонового потока для 1 слайдера.
    private void StartCheckBox1_Checked(object sender, RoutedEventArgs e)
    {
        // Создаем и запускаем поток.
        var th1 = new Thread(ThreadFunk1);
        th1.Name = "Thread1";
        th1.IsBackground = true;
        // Сбросить состояние события.
        event_for_stop1.Reset();
        th1.Start();
        StartLabel1.Content = "Остановить 1-й поток";
        StackPanelStop1.IsEnabled = true;
    }

    private void StartCheckBox1_Unchecked(object sender, RoutedEventArgs e)
    {
        StartLabel1.Content = "Запустить 1-й поток";
        StackPanelStop1.IsEnabled = false;
        CheckBoxStop1.IsChecked = false;

        // Установить событие в сигнальное состояние.
        event_for_stop1.Set();
    }

    private void CheckBoxStop1_Checked(object sender, RoutedEventArgs e)
    {
        LabelStop1.Content = "Возобновить 1-й поток";
        event_for_suspend1.Reset();
    }

    private void CheckBoxStop1_Unchecked(object sender, RoutedEventArgs e)
    {
        LabelStop1.Content = "Приостановить 1-й поток";
        event_for_suspend1.Set();
    }

    #endregion

    #region 2 поток (2 слайдер)

    private readonly ManualResetEvent event_for_stop2 = new(false);
    private readonly ManualResetEvent event_for_suspend2 = new(true);

    private void ThreadFunk2()
    {
        try
        {
            UiContext.Send(d => ProgressBar2.Minimum = 0, null);
            UiContext.Send(d => ProgressBar2.Maximum = 100, null);
            UiContext.Send(d => ProgressBar2.Value = 0, null);

            while (true)
            {
                if (event_for_stop2.WaitOne(0)) break;

                event_for_suspend2.WaitOne(); // Если сигналит усыпляет.
                Thread.Sleep(250);
                UiContext.Send(d => ProgressBar2.Value = GetRandomValue(), null);
            }
            // Возможно тут стоит сбросить прогрессбар в value = 0;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    // Пуск 2 фонового потока для 2 слайдера.
    private void StartCheckBox2_Checked(object sender, RoutedEventArgs e)
    {
        // Создаем и запускаем поток.
        var th2 = new Thread(ThreadFunk2);
        th2.Name = "Thread2";
        th2.IsBackground = true;
        // Сбросить состояние события.
        event_for_stop2.Reset();
        th2.Start();
        StartLabel2.Content = "Остановить 2-й поток";
        StackPanelStop2.IsEnabled = true;
    }

    private void StartCheckBox2_Unchecked(object sender, RoutedEventArgs e)
    {
        StartLabel2.Content = "Запустить 2-й поток";
        StackPanelStop2.IsEnabled = false;
        CheckBoxStop2.IsChecked = false;

        // Установить событие в сигнальное состояние.
        event_for_stop2.Set();
    }

    private void CheckBoxStop2_Checked(object sender, RoutedEventArgs e)
    {
        LabelStop2.Content = "Возобновить 2-й поток";
        event_for_suspend2.Reset();
    }

    private void CheckBoxStop2_Unchecked(object sender, RoutedEventArgs e)
    {
        LabelStop2.Content = "Приостановить 2-й поток";
        event_for_suspend2.Set();
    }

    #endregion

    #region 3 поток (3 слайдер)

    private readonly ManualResetEvent event_for_stop3 = new(false);
    private readonly ManualResetEvent event_for_suspend3 = new(true);

    private void ThreadFunk3()
    {
        try
        {
            UiContext.Send(d => ProgressBar3.Minimum = 0, null);
            UiContext.Send(d => ProgressBar3.Maximum = 100, null);
            UiContext.Send(d => ProgressBar3.Value = 0, null);

            while (true)
            {
                if (event_for_stop3.WaitOne(0)) break;

                event_for_suspend3.WaitOne(); // Если сигналит усыпляет.
                Thread.Sleep(250);
                UiContext.Send(d => ProgressBar3.Value = GetRandomValue(), null);
            }
            // Возможно тут стоит сбросить прогрессбар в value = 0;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    // Пуск 3 фонового потока для 3 слайдера.
    private void StartCheckBox3_Checked(object sender, RoutedEventArgs e)
    {
        // Создаем и запускаем поток.
        var th3 = new Thread(ThreadFunk3);
        th3.Name = "Thread3";
        th3.IsBackground = true;
        // Сбросить состояние события.
        event_for_stop3.Reset();
        th3.Start();
        StartLabel3.Content = "Остановить 3-й поток";
        StackPanelStop3.IsEnabled = true;
    }

    private void StartCheckBox3_Unchecked(object sender, RoutedEventArgs e)
    {
        StartLabel3.Content = "Запустить 3-й поток";
        StackPanelStop3.IsEnabled = false;
        CheckBoxStop3.IsChecked = false;

        // Установить событие в сигнальное состояние.
        event_for_stop3.Set();
    }

    private void CheckBoxStop3_Checked(object sender, RoutedEventArgs e)
    {
        LabelStop3.Content = "Возобновить 3-й поток";
        event_for_suspend3.Reset();
    }

    private void CheckBoxStop3_Unchecked(object sender, RoutedEventArgs e)
    {
        LabelStop3.Content = "Приостановить 3-й поток";
        event_for_suspend3.Set();
    }

    #endregion
}

// 20:24:35 событие с ручным сбросом