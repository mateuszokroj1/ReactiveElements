using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

using ReactiveElements;

namespace WpfApp1
{
    public class MainWindowViewModel
    {
        public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>();
        public ReadonlyReactiveProperty<string> Display { get; }

        public ReactiveProperty<int> Value { get; set; } = new ReactiveProperty<int>(0);

        public ReactiveCommand Command { get; }

        public MainWindowViewModel()
        {
            Display = Name.Select(text => text?.ToUpperInvariant()).ToReadonlyReactiveProperty();
            Command = new ReactiveCommand(() => App.Current.Shutdown(), Value.Select(val => val > 0));
        }
    }
}
