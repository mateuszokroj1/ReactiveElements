using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveElements.Observable
{
    public class ObservableCollector<Tout> : IDisposable, IObservable<Tout>
    {
        private readonly List<IObserver<Tout>> observers = new List<IObserver<Tout>>();
        private IDisposable[] disposables;
        private dynamic[] currentValues;
        private 

        public ObservableCollector(Func<dynamic[],Tout> calculationFunction, params IObservable<dynamic>[] observables)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<Tout> observer)
        {
            throw new NotImplementedException();
        }
    }
}
