using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common
{
    public abstract class ObservableObject<T>
    {
        public delegate void NotifyObserver(T item);
        public event NotifyObserver NotifyObserverEvent;

        public void AddObserver(NotifyObserver o)
        {
            NotifyObserverEvent += o;
        }

        public void RemoveObserver(NotifyObserver o)
        {
            NotifyObserverEvent -= o;
        }

        public void Notify(T item)
        {
            if (NotifyObserverEvent != null)
            {
                NotifyObserverEvent(item);
            }
        }
    }
}
