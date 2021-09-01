using Moq;

using ReactiveElements.Observable;

using System;
using System.Collections.Generic;

using Xunit;

namespace ReactiveElements.Tests.Disposables
{
    public class UnsubscriberTest
    {
        [Fact]
        public void Constructor_WhenArgumentIsNull_ShouldThrowException()
        {
            IList<IObserver<object>> observersNull = null;
            IObserver<object> observerNull = null;

            var observers = new List<IObserver<object>>();
            var observer = Mock.Of<IObserver<object>>(MockBehavior.Loose);

            Assert.Throws<ArgumentNullException>(() => new Unsubscriber<object>(observersNull, observer));
            Assert.Throws<ArgumentNullException>(() => new Unsubscriber<object>(observers, observerNull));
        }

        [Fact]
        public void Constructor_WhenArgumentsIsValid_ShouldNotThrow()
        {
            var observer = Mock.Of<IObserver<bool>>(MockBehavior.Loose);
            var observers = new List<IObserver<bool>> { observer };

            Assert.NotNull(new Unsubscriber<bool>(observers, observer));
        }

        [Fact]
        public void Dispose_ShouldRemoveObserverFromCollection()
        {
            var observer = Mock.Of<IObserver<int>>(MockBehavior.Loose);
            var observers = new List<IObserver<int>> { observer };

            var result = new Unsubscriber<int>(observers, observer);

            result.Dispose();

            Assert.Empty(observers);
        }
    }
}
