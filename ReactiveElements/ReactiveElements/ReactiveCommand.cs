using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace ReactiveElements
{
    public class ReactiveCommand : ICommand, IObserver<bool>, IDisposable
    {
        #region Fields

        public event EventHandler CanExecuteChanged;
        private Action<object> toExecute;
        private bool canExecute;
        private IDisposable unsubscriber;

        #endregion

        #region Constructor

        public ReactiveCommand(Action<object> execute, IObservable<bool> canExecute)
        {
            this.toExecute = execute;
            this.unsubscriber = canExecute.Subscribe(this);
        }

        public ReactiveCommand(Action<object> execute, Func<bool> canExecute, int interval)
        {
            this.toExecute = execute;
            this.unsubscriber = 
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter) => this.canExecute;

        public void Execute(object parameter) => this.toExecute(parameter);

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(bool value)
        {
            this.canExecute = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            this.unsubscriber?.Dispose();
        }

        ~ReactiveCommand()
        {
            Dispose();
        }

        #endregion
    }
}
