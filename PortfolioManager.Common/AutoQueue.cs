using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Collections.Concurrent;

namespace PortfolioManager.Common
{
    public delegate void AutoQueueEventHandler<T>(T eventArgs);

    /// <summary>
    /// Wrapper on Concurrent Queue having capability to Auto Dequeue all items after certain period of time
    /// Can be used to conflate messages and throttle the speed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoQueue<T>:ConcurrentQueue<T>
    {
        public event AutoQueueEventHandler<IEnumerable<GenericsEventArgs<T>>> signalEvent;
        
        private Timer m_timer;
        private int _interval;

        public AutoQueue(int interval)
        {
            _interval = interval;
            m_timer = new Timer(new TimerCallback(TimerProc));
        }

        public void Start()
        {
            m_timer.Change(_interval, _interval);
        }

        public void Stop()
        {
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void TimerProc(object state)
        {
            Stop();
            signalEvent.Invoke(/*this, */DequeuAll().Select(x => new GenericsEventArgs<T>(x)));
            Start();
        }

        private IEnumerable<T> DequeuAll()
        {
            while (this.Count != 0)
            {
                T item;
                this.TryDequeue(out item);
                yield return item;
            };
        }
    }
}
