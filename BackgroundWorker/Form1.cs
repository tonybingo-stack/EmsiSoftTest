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
using BackgroundWorker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackgroundWorker
{
    public partial class Form1 : Form
    {
        ConnectionFactory connectionFactory;
        IConnection connection;
        IModel channel;
        EventingBasicConsumer consumer;
        List<string> hashList = new List<string>();
        int hashCount = 0;

        public Form1()
        {
            InitializeComponent();

            connectionFactory = new ConnectionFactory { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // save hash to db
                hashList.Add(message);
                hashCount++;

                if (!backgroundWorker2.IsBusy)
                {
                    backgroundWorker2.RunWorkerAsync(0);
                }
            };
            channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        }

        private void start_Click(object sender, EventArgs e)
        {
            //if (!backgroundWorker2.IsBusy)
            //    backgroundWorker2.RunWorkerAsync("ok");
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
            //if (e.Cancelled) MessageBox.Show("Operation was canceled");
            //else if (e.Error != null) MessageBox.Show(e.Error.Message);
            //else MessageBox.Show(e.Result.ToString());
        }

        private int BackgroundProcessLogicMethod(System.ComponentModel.BackgroundWorker bw, int a)
        {
            for(int i = 0; i < hashCount; i++)
            {
                bool result = SaveHash(hashList[i]);
            }
            return 0;
        }
        public bool SaveHash(string hashCode) // calling SaveStudentMethod for insert a new record
        {
            hash h = new hash();
            h.id = Guid.NewGuid();
            h.date = DateTime.Now;
            h.sha1 = hashCode;

            bool result = false;
            using (HashConnectionStr _entity = new HashConnectionStr())
            {
                //_entity.hashes.Add(d);
                _entity.hashes.Add(h);
                _entity.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
