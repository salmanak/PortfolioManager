using System;
namespace PortfolioManager.Common
{
    public interface IObservableObject<T>
    {
        void AddObserver(ObservableObject<T>.NotifyObserver o);
        void Notify(T item);
        void RemoveObserver(ObservableObject<T>.NotifyObserver o);
    }
}
