using System;

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
        public void Constructor1_WhenArgumentsIsValid_ShouldBeInitialized()
        {
            bool executed = false;
            bool eventTriggered = false;
            var canExecuteProperty = new ReactiveProperty<bool>(false);
            var command = new ReactiveCommand(() => executed = true, canExecuteProperty);
            command.CanExecuteChanged += (sender, e) => eventTriggered = true;

            Assert.False(command.CanExecute(null));

            canExecuteProperty.Value = true;
            Assert.True(command.CanExecute(null));
            Assert.True(eventTriggered);

            command.Execute(null);
            Assert.True(executed);
        }

        [Fact]
        public void Constructor2_WhenArgumentsIsValid_ShouldBeInitialized()
        {
            bool executed = false;
            bool eventTriggered = false;
            var canExecuteProperty = new ReactiveProperty<bool>(false);
            var command = new ReactiveCommand(parameter => executed = true, canExecuteProperty);
            command.CanExecuteChanged += (sender, e) => eventTriggered = true;

            Assert.False(command.CanExecute(null));

            canExecuteProperty.Value = true;
            Assert.True(command.CanExecute(null));
            Assert.True(eventTriggered);

            command.Execute(null);
            Assert.True(executed);
        }

        #endregion

        #region Methods

        [Fact]
        public void CanExecute_ShouldReturnBoolWithoutParameter()
        {
            bool executed = false;
            var command = new ReactiveCommand(() => executed = true, new ReactiveProperty<bool>(true));
            Assert.True(command.CanExecute(null));
            Assert.True(command.CanExecute(324));

            command = new ReactiveCommand(parameter => executed = true, new ReactiveProperty<bool>(false));
            Assert.False(command.CanExecute(null));
            Assert.False(command.CanExecute(""));
        }

        [Fact]
        public void Execute_ShouldRun()
        {
            bool executed = false;
            var command = new ReactiveCommand(() => executed = true, new ReactiveProperty<bool>(true));
            command.Execute(null);
            Assert.True(executed);

            executed = false;
            command = new ReactiveCommand(parameter => executed = true, new ReactiveProperty<bool>(false));
            command.Execute(null);
            command.CanExecute("");
            Assert.True(executed);
        }

        [Fact]
        public void OnNext_ShouldChangeCanExecuteValue()
        {
            bool executed = false;
            var command = new ReactiveCommand(() => executed = true, new ReactiveProperty<bool>(true));
            Assert.True(command.CanExecute(null));

            command.OnNext(false);
            Assert.False(command.CanExecute(""));

            command.OnNext(true);
            Assert.True(command.CanExecute(12));
        }

        [Fact]
        public void OnCompleted_ShouldNotThrow()
        {
            bool executed = false;
            var command = new ReactiveCommand(() => executed = true, new ReactiveProperty<bool>(true));
            command.OnCompleted();
        }

        [Fact]
        public void OnError_ShouldSetLastError()
        {
            bool executed = false;
            ReactiveCommand command = new ReactiveCommand(() => executed = true, new ReactiveProperty<bool>(true));
            var exception = new ApplicationException("TEST");
            command.OnError(exception);
        }

        #endregion
    }
}
