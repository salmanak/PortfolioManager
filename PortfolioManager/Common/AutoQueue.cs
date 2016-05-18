using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Collections.Concurrent;

namespace PortfolioManager.Common
{  
    public delegate void AutoQueueEventHandler<T, U>(T sender, U eventArgs);
    public class AutoQueue<T>:ConcurrentQueue<T>
    {
        public class GenericsEventArgs<T> : EventArgs
        {
            public GenericsEventArgs(T item)
            {
                Item = item;
            }

            public T Item { get; protected set; }

            public static implicit operator GenericsEventArgs<T>(T item)
            {
                return new GenericsEventArgs<T>(item);
            }
        }

        public event AutoQueueEventHandler<AutoQueue<T>, IEnumerable<GenericsEventArgs<T>>> signalEvent;
        protected virtual void OnSignal(IEnumerable<GenericsEventArgs<T>> a)
        {
            signalEvent(this, a);
        }
        
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
            signalEvent.Invoke(this, DequeuAll().Select(x => new GenericsEventArgs<T>(x)));
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
