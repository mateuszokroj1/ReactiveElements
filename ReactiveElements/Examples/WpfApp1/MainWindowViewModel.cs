using System;
using System.Reactive.Linq;

using ReactiveElements;
using ReactiveElements.Observable;

namespace WpfApp1
{
    public class MainWindowViewModel
    {
        public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>();
        public ReadonlyReactiveProperty<string> Display { get; }

        public ReactiveProperty<int> Value { get; set; } = new ReactiveProperty<int>(0);

        public ReactiveCommand Command { get; }

        public ReadonlyReactiveProperty<bool> IsChecked { get; }

        public MainWindowViewModel()
        {
            Display = new ReadonlyReactiveProperty<string>(Name.Select(text => text?.ToUpperInvariant()));

            Command = new ReactiveCommand(() => App.Current.Shutdown(), Value.Select(value => value > 0));

            IsChecked = new TimedValueComparer<int>(() => DateTime.Now.Second, TimeSpan.FromSeconds(1))
                .Select(seconds => seconds % 2 == 0)
                .ToReadonlyReactiveProperty();
        }
    }
}
