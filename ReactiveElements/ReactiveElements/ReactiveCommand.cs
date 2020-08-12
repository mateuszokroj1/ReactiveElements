using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace ReactiveElements
{
    public sealed class ReactiveCommand : ICommand, IDisposable
    {
        #region Fields

        public event EventHandler CanExecuteChanged;
        private readonly Action<object> toExecute;
        private bool canExecute;
        private readonly IDisposable unsubscriber;

        #endregion

        #region Constructor

        public ReactiveCommand(Action<object> execute, IObservable<bool> canExecute)
        {
            this.toExecute = execute;
            this.unsubscriber = canExecute.Subscribe(value => OnNext(value));
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter) => this.canExecute;

        public void Execute(object parameter) => this.toExecute(parameter);

        private void OnNext(bool value)
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
