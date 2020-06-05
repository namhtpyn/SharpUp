using SharpUp.Log;
using SharpUp.Oracle;
using SharpUp.ScheduleJob;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private Log log = new Log("./logs", LogType.Info | LogType.Warning | LogType.Error);
        private ScheduleJob job = new ScheduleJob();

        public Form1()
        {
            InitializeComponent();
            log.OnWrite += Log_OnWrite;
            job.Add(DoLog, DateTime.Now, TimeSpan.FromSeconds(1));
        }

        private void Log_OnWrite(object sender, LogEntry e)
        {
            richTextBox1.SuspendLayout();
            richTextBox1.Invoke((MethodInvoker)(() =>
            {
                richTextBox1.AppendText(e.ToString() + Environment.NewLine);
            }));
            richTextBox1.ResumeLayout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OracleConnection connection = new OracleConnection("Data Source = (DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.157.6.5)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xNetDB.vnpt.vn))); User Id=xNet; Password=Mznxbc951; Max Pool Size=300");
            connection.Open();
            var x = connection.CreateProcCommand("XNET.TEST_API.TEST_PRC", null, 1, OracleDbType.RefCursor).ExecuteList();
            //var cmd = connection.CreateProcCommand("XNET.TEST_API.TEST_PRC");
            //cmd.Parameters.Add(new OracleParameter(1));
            //var y = cmd.ExecuteList();
            connection.Close();
        }

        private string camel(string input)
        {
            return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        }

        private void DoLog(object obj)
        {
            log.Info("info");
            log.Success("Success");
            log.Warning("Warning");
            log.Error("Error");
            Thread.Sleep(10000);
        }
    }
}