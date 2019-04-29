using System;
using System.Threading;

namespace WindowsFormsApp1
{
    public class Worker
    {
        SynchronizationContext _context;
        public Worker(SynchronizationContext context )
        {
            _context = context;
        }

        //Flag for canceling work
        private bool _canceled = false;
        //Events
        //Change progress bar value
        public event Action<int> ProgressChanged;
        //Show message about result of this work
        public event Action<bool> WorkCompleted;

        public void Cancel()
        {
            _canceled = true;
        }

        //Do work (changing progress bar value in time)
        public void Work(int maxValue)
        {
            _canceled = false;
            for (int i = 1; i <= maxValue; i++)
            {
                if (_canceled) break;
                //Change event !!
                _context.Send(OnProgressChanged, i);
                Thread.Sleep(50);
            }
            //Complete event !!
            _context.Send(OnWorkCompleted, _canceled);
        }

        public void OnProgressChanged(object i)
        {
            ProgressChanged?.Invoke((int)i);
        }

        public void OnWorkCompleted(object canceled)
        {
            WorkCompleted?.Invoke((bool)canceled);
        }
    }
}
