using SharpUp.Log;
using SharpUp.ScheduleJob;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private ScheduleJob job = new ScheduleJob();
        private int counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new JobTimer(testJob, DateTime.Now, TimeSpan.FromSeconds(3));
            job.Add(testJob, DateTime.Now, TimeSpan.FromSeconds(3));
        }

        private async Task testJob()
        {
            counter++;
            var id = counter;
            //log.Info($"in {id}");
            await Task.Delay(5000);
            //log.Info($"out {id}");
        }
    }
}