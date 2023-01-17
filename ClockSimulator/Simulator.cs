using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClockSimulator
{
    internal static class Simulator
    {
        public static BackgroundWorker orderWorker = new BackgroundWorker();

        static List<Order> orders = new List<Order>();

        static Simulator()
        {

            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                orders.Add(new Order()
                    {
                        Id = i,
                        OrderDate = DateTime.Now.AddDays(-1 * random.Next(30) )
                    }
                );
            }
        }
        public static void StartSimulator()
        {
            orderWorker.DoWork += OrderWorker_DoWork;

           // orderWorker.ProgressChanged += OrderWorker_ProgressChanged;
            orderWorker.WorkerReportsProgress = true;

            orderWorker.WorkerSupportsCancellation = true;

            orderWorker.RunWorkerAsync();
        }

        private static void OrderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void OrderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!orderWorker.CancellationPending)
            {
                Thread.Sleep(1000);
                updateOrder();


            }
        }

        private static void updateOrder()
        {
            Order ord = orders.OrderBy(order => (DateTime.Now - order.OrderDate).Ticks).FirstOrDefault();
            ord.OrderDate = DateTime.Now;
            orders.Add(ord);

            orderWorker.ReportProgress(1, ord);
        }
    }
}
