using System;
using System.Numerics;
using System.Reactive.Linq;

using Xunit;

namespace ReactiveElements.Tests
{
    public class ReadonlyReactivePropertyTest
    {
        #region Constructors

        [Fact]
        public void Constructor1_ShouldSetValueToDefault()
        {
            Assert.Equal(default, new ReadonlyReactiveProperty<bool>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<byte>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<sbyte>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<short>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<ushort>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<int>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<uint>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<long>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<ulong>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<float>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<double>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<decimal>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<Complex>().Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<BigInteger>().Value);
        }

        [Fact]
        public void Constructor2_ShouldSetValue()
        {
            Assert.Equal(default, new ReadonlyReactiveProperty<bool>(default(bool)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<byte>(default(byte)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<sbyte>(default(sbyte)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<short>(default(short)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<ushort>(default(ushort)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<int>(default(int)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<uint>(default(uint)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<long>(default(long)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<ulong>(default(ulong)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<float>(default(float)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<double>(default(double)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<decimal>(default(decimal)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<Complex>(default(Complex)).Value);
            Assert.Equal(default, new ReadonlyReactiveProperty<BigInteger>(default(BigInteger)).Value);
        }

        #region Constructor3

        [Fact]
        public void Constructor3_WhenValueIsValid_ShouldSubscribeSource()
        {
            var property = new ReadonlyReactiveProperty<int>(System.Reactive.Linq.Observable.Range(10, 1));
            Assert.Equal(10, property);
        }

        [Fact]
        public void Constructor3_WhenValueIsNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReadonlyReactiveProperty<bool>(null));
        }

        #endregion

        #endregion

        #region Property_Value

        [Fact]
        public void Value_Get_ShouldReturnActualValue()
        {
            var property = new ReadonlyReactiveProperty<int>(1);
            Assert.Equal(1, property.Value);
        }

        #endregion

        #region GetValue

        [Fact]
        public void GetValue_ShouldReturnActualValue()
        {
            var property = new ReadonlyReactiveProperty<double>(0.09123);
            Assert.Equal(0.09123, property.GetValue());
        }

        #endregion

        #region Subscribe

        [Fact]
        public void Subscribe_WhenArgumentIsNotNull_ShouldReturnIDisposable()
        {
            var property = new ReadonlyReactiveProperty<float>(0.5f);
            Assert.IsAssignableFrom<IDisposable>(property.Subscribe());
        }

        [Fact]
        public void Subscribe_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            var property = new ReadonlyReactiveProperty<long>(100L);
            IObserver<long> observer = null;
            Assert.Throws<NullReferenceException>(() => property.Subscribe(observer));
        }

        #endregion

        #region Dispose

        [Fact]
        public void Dispose_ShouldDisposeObject()
        {
            var property = new ReadonlyReactiveProperty<bool>();
            bool runned = false;
            bool val;
            property.Subscribe(newVal => val = newVal, () => runned = true);
            property.Dispose();
            Assert.True(runned);
        }

        #endregion

        #region GetTypeCode

        [Fact]
        public void GetTypeCode_ShouldReturnValidTypeCode()
        {
            ReadonlyReactiveProperty<bool> testProperty = new ReadonlyReactiveProperty<bool>();
            Assert.Equal(TypeCode.Object, testProperty.GetTypeCode());
        }

        #endregion

        #region IObserver

        [Fact]
        public void OnNext_WhenListHaveObserver_ShouldSetValueAndNotifyObserver()
        {
            int receivedValue = 0;
            var property = new ReadonlyReactiveProperty<int>();
            property.Subscribe(newVal => receivedValue = newVal);
            property.OnNext(10);
            Assert.Equal(10, property);
            Assert.Equal(10, receivedValue);
        }

        [Fact]
        public void OnNext_WhenListIsEmpty_ShouldSetValue()
        {
            var property = new ReadonlyReactiveProperty<int>();
            property.OnNext(10);
            Assert.Equal(10, property);
        }

        [Fact]
        public void OnCompleted_DoNothing()
        {
            var property = new ReadonlyReactiveProperty<bool>();
            bool runned = false;
            bool value;
            property.Subscribe(val => value = val, () => runned = true);
            property.OnCompleted();

            Assert.True(runned);
        }

        [Fact]
        public void OnError_ShouldNotifyObserver()
        {
            var property = new ReadonlyReactiveProperty<int>(1);
            int val;
            Exception exception = null;
            property.Subscribe(newVal => val = newVal, exc => exception = exc, () => val = 2);
            property.OnError(new ApplicationException("TEST"));

            Assert.IsType<ApplicationException>(exception);
        }

        #endregion

        #region Implicit operator

        [Fact]
        public void ImplicitOperator_ShouldReturnActualValue()
        {
            var property = new ReadonlyReactiveProperty<double>(Math.PI);
            Assert.Equal(Math.PI, property);
        }

        #endregion
    }
}
