using System;
using System.Threading;

namespace ScrumPoker.API.TimerFeatures
{
    public class TimerManager
    {
        private Timer _timer { get; set; }
        private AutoResetEvent _autoResetEvent { get; set; }
        private Action _action { get; set; }
        public DateTime TimerStarted { get; set; }

        public TimerManager(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
            TimerStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            _action();

            if ((DateTime.Now - TimerStarted).Seconds > 60)
            {
                _timer.Dispose();
            }
        }

    }
}