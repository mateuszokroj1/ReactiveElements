using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using ReactiveElements.Observable;

using Xunit;

namespace ReactiveElements.Tests.Observable
{
    public class TimedValueComparerTest
    {
        #region Constructors

        [Fact]
        public void Constructor_ShouldInitializeAndSetValue()
        {
            var timedValueComparer = new TimedValueComparer<double>(() => Math.PI, TimeSpan.FromMilliseconds(10));
            Assert.Equal(Math.PI, timedValueComparer.CurrentValue);
        }

        #endregion

        #region Properties

        [Fact]
        public void SynchronizationContext_Get_ShouldReturnDefaultValue()
        {
            var timedValueComparer = new TimedValueComparer<bool>(() => true, TimeSpan.FromSeconds(10));
            Assert.Equal(SynchronizationContext.Current, timedValueComparer.SynchronizationContext);
            timedValueComparer.Dispose();
        }

        

        #endregion
    }
}
