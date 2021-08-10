using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ReactiveElements.Tests
{
    internal class TestModel : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        private string property1;
        private int property2;

        #endregion

        #region Properties

        public string Property1
        {
            get => this.property1;
            set => SetProperty(ref this.property1, value);
        }

        public int Property2
        {
            get => this.property2;
            set => SetProperty(ref this.property2, value);
        }

        #endregion

        #region Methods

        private void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, newValue))
            {
                field = newValue;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
