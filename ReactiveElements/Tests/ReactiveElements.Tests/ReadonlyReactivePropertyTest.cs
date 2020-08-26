using System;

using ReactiveElements;

using Xunit;

namespace ReactiveElements.Tests
{
    public class ReadonlyReactivePropertyTest
    {
        #region Constructors

        [Fact]
        public void Constructor1_ShouldNotThrowAndSetValueToDefault()
        {

        }

        #endregion

        #region Property_Value

        [Fact]
        public void Value_Get_ShouldReturnActualValue()
        {

        }

        #endregion

        #region GetValue

        [Fact]
        public void GetValue_ShouldReturnActualValue()
        {

        }

        #endregion

        #region Subscribe

        [Fact]
        public void Subscribe_WhenArgumentIsNotNull_ShouldReturnIDisposable()
        {

        }

        [Fact]
        public void Subscribe_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {

        }

        #endregion

        #region Dispose

        [Fact]
        public void Dispose_ShouldDisposeObject()
        {

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

        #region IConvert


        #endregion

        #region IObserver


        #endregion

        #region Implicit operator

        [Fact]
        public void ImplicitOperator_ShouldReturnActualValue()
        {

        }

        #endregion
    }
}
