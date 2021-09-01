using System.ComponentModel;

namespace ReactiveElements.Tests.Fixtures
{
    internal class TestModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public object Property { get; set; }
    }
}
