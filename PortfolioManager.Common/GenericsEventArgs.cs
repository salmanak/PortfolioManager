using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common
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
}
