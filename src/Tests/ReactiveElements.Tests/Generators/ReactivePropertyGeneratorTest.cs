using ReactiveElements.Generators;
using ReactiveElements.Tests.Fixtures;

using System;
using System.Reflection;

using Xunit;

namespace ReactiveElements.Tests.Generators
{
    public class ReactivePropertyGeneratorTest
    {
        [Fact]
        public void Generate_WhenArgumentIsNull_ShouldThrowException()
        {
            TestModel modelNull = null;
            PropertyInfo propertyInfoNull = null;

            var model = new TestModel();
            var propertyInfo = model.GetType().GetProperty(nameof(model.Property));

            Assert.Throws<ArgumentNullException>(() => ReactivePropertyGenerator.GenerateReactivePropertyFromPropertyChangedEventModel<TestModel, object>(modelNull, propertyInfo));
            Assert.Throws<ArgumentNullException>(() => ReactivePropertyGenerator.GenerateReactivePropertyFromPropertyChangedEventModel<TestModel, object>(model, propertyInfoNull));
        }

        [Fact]
        public void Generate_WhenArgumentsIsValid_ShouldReturnValidValue()
        {
            var value = new object();
            var model = new TestModel() { Property = value };
            var propertyInfo = model.GetType().GetProperty(nameof(model.Property));

            var result = ReactivePropertyGenerator.GenerateReactivePropertyFromPropertyChangedEventModel<TestModel, object>(model, propertyInfo);

            Assert.NotNull(result);
            Assert.Equal(model.Property, result.Value);
        }
    }
}
