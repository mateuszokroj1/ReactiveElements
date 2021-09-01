using ReactiveElements.Helpers;
using ReactiveElements.Tests.Fixtures;

using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace ReactiveElements.Tests.Helpers
{
    public class PropertyInfoExtractorTest
    {
        [Fact]
        public void Extract_WhenArgument1IsNull_ShouldThrowException()
        {
            TestModel model = null;

            Assert.Throws<ArgumentNullException>(() => PropertyInfoExtractor.Extract(model, model => model.Property));
        }

        [Fact]
        public void Extract_WhenArgument2IsNull_ShouldThrowException()
        {
            var model = new TestModel();

            Assert.Throws<ArgumentException>(() => PropertyInfoExtractor.Extract<TestModel, object>(model, null));
        }

        [Fact]
        public void Extract_ShouldReturnPropertyInfo()
        {
            var model = new TestModel();

            var result = PropertyInfoExtractor.Extract(model, model => model.Property);

            Assert.NotNull(result);
            Assert.Equal(nameof(model.Property), result.Name);
            Assert.Equal(typeof(object), result.PropertyType);
        }
    }
}
