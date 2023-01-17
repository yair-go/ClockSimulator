using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClockSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        BackgroundWorker clockWorker;      

        DateTime display = DateTime.Now;
        private int rate = 1;

        public MainWindow()
        {
            InitializeComponent();

            ShowTime();

            clockWorker = new BackgroundWorker();
            clockWorker.DoWork += ClockWorker_DoWork;
            
            clockWorker.ProgressChanged += ClockWorker_UpdateDisplay;
            clockWorker.WorkerReportsProgress = true;

            clockWorker.WorkerSupportsCancellation = true;
            clockWorker.RunWorkerCompleted += ClockWorker_RunWorkerCompleted;

            clockWorker.RunWorkerAsync(1);
        }

        private void ClockWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int sec = rate;
            clockWorker.RunWorkerAsync(sec);
        }

        private void ShowTime()
        {
            string timerText = display.ToString();
            //           timerText = timerText.Substring(10, 18);
            this.timeTextBlock.Text = timerText;
        }

        private void ClockWorker_UpdateDisplay(object sender, ProgressChangedEventArgs e)
        {
            display = display.AddSeconds(e.ProgressPercentage);
            ShowTime();
        }

        private void ClockWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int sec = (int)e.Argument; // sec = 1

            Simulator.orderWorker.ProgressChanged += OrderWorker_ProgressChanged;

            while (!clockWorker.CancellationPending) // while (true);
            {
                Thread.Sleep(1000);
                clockWorker.ReportProgress(sec);      
            }
        }

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            rate = 8;
            if (clockWorker.WorkerSupportsCancellation == true)
                clockWorker.CancelAsync(); // Cancel the asynchronous operation.
            
            Simulator.StartSimulator();
            
        }

        private void OrderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void stopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            rate = 1;
            if (clockWorker.WorkerSupportsCancellation == true)
                clockWorker.CancelAsync(); // Cancel the asynchronous operation.

            Simulator.orderWorker.CancelAsync();
        }
    }
}
