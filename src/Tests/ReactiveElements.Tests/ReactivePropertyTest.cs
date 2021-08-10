using System;

using Xunit;


namespace ReactiveElements.Tests
{
    public class ReactivePropertyTest
    {
        #region Properties

        [Fact]
        public void Value_ShouldSetValueAndNotifyObserver()
        {
            var property = new ReactiveProperty<int>(0);
            int val;
            property.Subscribe(newVal => val = newVal);
            property.Value = 3;
            Assert.Equal(3, property.Value);
        }

        #endregion

        #region Set/Get

        [Fact]
        public void SetValue_ShouldSetValueAndNotifyObserver()
        {
            var property = new ReactiveProperty<int>(0);
            int val;
            property.Subscribe(newVal => val = newVal);
            property.SetValue(5);
            Assert.Equal(5, property.GetValue());
        }

        #endregion

        #region FromBindableModel

        [Fact]
        public void FromBindableModel_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReactiveProperty<bool>.FromBindableModel<ReactiveProperty<bool>>(null, t => t.Value));
            Assert.Throws<ArgumentNullException>(() => ReactiveProperty<bool>.FromBindableModel(new ReactiveProperty<bool>(), null));
        }

        [Fact]
        public void FromBindableModel_WhenArgumentsIsValid_ShouldReturnReactivePropertyAnd()
        {
            TestModel model = new TestModel();
            model.Property1 = string.Empty;

            var property = ReactiveProperty<string>.FromBindableModel(model, m => m.Property1);
            Assert.IsType<ReactiveProperty<string>>(property);
            string val = null;
            property.Subscribe(newVal => val = newVal);
            model.Property1 = "TEST";
            Assert.Equal(model.Property1, val);
        }

        #endregion
    }
}
