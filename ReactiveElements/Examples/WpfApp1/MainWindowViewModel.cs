using System;
using System.Collections.Generic;
using System.Text;

using ReactiveElements;

namespace WpfApp1
{
    public class MainWindowViewModel
    {
        public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>();
        public IObservable<string> Display { get; }

        public ReactiveProperty<int> Value { get; set; } = new ReactiveProperty<int>(0);

        public ReactiveCommand Command { get; }

        public MainWindowViewModel()
        {
            Display = Name.Select(text => text?.ToUpperInvariant());

            //Command = new ReactiveCommand(() => App.Current.Shutdown(), Value);
        }
    }
}
