using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace ReactiveElements.Tests
{
    public class ExtensionMethodsTest
    {
        #region GetReactiveProperty

        [Fact]
        public void GetReactiveProperty_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ExtensionMethods.GetReactiveProperty<TestModel, string>(null, model => model.Property1));
            Assert.Throws<ArgumentNullException>(() => ExtensionMethods.GetReactiveProperty<TestModel, string>(new TestModel(), null));
        }

        [Fact]
        public void GetReactiveProperty_WhenArgumentsIsValid_ShouldReturnReactiveProperty()
        {
            var model = new TestModel();
            var property = model.GetReactiveProperty(m => m.Property1);
            Assert.IsType<ReactiveProperty<string>>(property);
            Assert.Equal(model.Property1, property.Value);
        }

        #endregion

        #region ToReadonlyReactiveProperty

        [Fact]
        public void ToReadonlyReactiveProperty_ShouldReturnReadonlyReactiveProperty()
        {
            ReadonlyReactiveProperty<int> property = System.Reactive.Linq.Observable.Range(0, 2).ToReadonlyReactiveProperty();
            Assert.NotNull(property);
        }

        #endregion

        #region ToReactiveProperty

        [Fact]
        public void ToReactiveProperty_ShouldReturnReactiveProperty()
        {
            ReactiveProperty<int> property = System.Reactive.Linq.Observable.Range(0, 2).ToReactiveProperty();
            Assert.NotNull(property);
        }

        #endregion
    }
}
