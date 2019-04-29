using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ProgressForm : Form
    {
        Worker _worker;
        SynchronizationContext _context;

        public ProgressForm()
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;

            //Buttons clicks
            btn_start.Click += Btn_start_Click;
            btn_cancel.Click += Btn_cancel_Click;

            //Worker init
            _worker = new Worker(_context);
            _worker.ProgressChanged += event_ProgressChanged;
            _worker.WorkCompleted += event_WokrComplete;
        }

        public void StartWork()
        {
            _worker.Work(progressBar.Maximum);
        }

        #region Buttons events
        private void Btn_start_Click(object sender, EventArgs e)
        {
            //Disable start button before work
            btn_start.Enabled = false;
            //Start work
            //Thread thread = new Thread(StartWork);
            //thread.Start();
            Task.Run(StartWork);
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            _worker.Cancel();
        }
        #endregion
        #region Worker events
        public void event_ProgressChanged(int value)
        {
            progressBar.Value = value;

            //Without SynchronizationContext
            //Run this action in Form thread
            //if (InvokeRequired)
            //    Invoke(action);
            //else
            //    action();
        }

        public void event_WokrComplete(bool canceled)
        {
            if (canceled)
                MessageBox.Show("Work canceled.");
            else
                MessageBox.Show("Work complete!");
            //Reset progress
            progressBar.Value = 0;
            btn_start.Enabled = true;


            //Without SynchronizationContext
            //Run this action in Form thread
            //if (InvokeRequired)
            //    Invoke(action);
            //else
            //    action();
        }
        #endregion
    }
}
