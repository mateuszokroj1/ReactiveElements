using System;
using System.Windows.Input;

namespace ReactiveElements
{
    public sealed class ReactiveCommand : ICommand, IDisposable, IObserver<bool>
    {
        #region Fields

        /// <summary>
        /// Notifies about CanExecute change.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private readonly Action<object> toExecute;
        private bool canExecute;
        private bool disposedValue;
        private readonly IDisposable unsubscriber;

        #endregion

        #region Constructor

        public ReactiveCommand(Action<object> execute, IObservable<bool> canExecute)
        {
            this.toExecute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.unsubscriber = canExecute?.Subscribe(this) ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public ReactiveCommand(Action execute, IObservable<bool> canExecute)
            : this(param => execute(), canExecute) { }

        #endregion

        #region Properties

        public Exception LastError { get; private set; }

        #endregion

        #region Methods

        public bool CanExecute(object parameter) => this.canExecute;

        public void Execute(object parameter) => this.toExecute(parameter);

        public void OnNext(bool value)
        {
            if (this.canExecute != value)
            {
                this.canExecute = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnCompleted() => this.unsubscriber?.Dispose();

        public void OnError(Exception error)
        {
            this.unsubscriber?.Dispose();
            LastError = error;
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.unsubscriber?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
