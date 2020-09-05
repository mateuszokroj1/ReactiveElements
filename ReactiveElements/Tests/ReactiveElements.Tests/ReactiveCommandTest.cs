using System;
using System.Reactive.Linq;

using Xunit;

namespace ReactiveElements.Tests
{
    public class ReactiveCommandTest
    {
        #region Constructor

        [Fact]
        public void Constructor_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            object argument;
            Assert.Throws<ArgumentNullException>(() => new ReactiveCommand(arg => argument = arg, null));
            Action<object> action1 = null;
            Action action2 = null;
            Assert.Throws<ArgumentNullException>(() => new ReactiveCommand(action1, new ReadonlyReactiveProperty<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => new ReactiveCommand(action2, new ReadonlyReactiveProperty<bool>(true)));
        }

        [Fact]
        public void Constructor_WhenArgumentsIsValid_ShouldInitializeProperly()
        {

        }

        #endregion
    }
}
