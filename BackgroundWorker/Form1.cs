using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker2.IsBusy)
                backgroundWorker2.RunWorkerAsync(2000);
        }

        private void end_Click(object sender, EventArgs e)
        {
            if (backgroundWorker2.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker2.CancelAsync();
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker helperBW = sender as System.ComponentModel.BackgroundWorker;
            int arg = (int)e.Argument;
            e.Result = BackgroundProcessLogicMethod(helperBW, arg);
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled) MessageBox.Show("Operation was canceled");
            else if (e.Error != null) MessageBox.Show(e.Error.Message);
            else MessageBox.Show(e.Result.ToString());
        }

        private int BackgroundProcessLogicMethod(System.ComponentModel.BackgroundWorker bw, int a)
        {
            int result = 0;
            Thread.Sleep(10000);
            MessageBox.Show("I was doing some work in the background.");
            return result;
        }
    }
}
