using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveElements;

namespace WpfApp2
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Display = FirstName.Select(name =>$"{name ?? string.Empty} {LastName.Value ?? string.Empty}").ToReadonlyReactiveProperty();
        }

        public ReactiveProperty<string> FirstName { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> LastName { get; } = new ReactiveProperty<string>();

        public ReadonlyReactiveProperty<string> Display { get; }
    }
}
