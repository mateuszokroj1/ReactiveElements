using System.Reflection;
using System.Windows;

namespace ReactiveElements.Wpf
{
    public class Binding : System.Windows.Data.Binding
    {
        public Binding() : base()
        {
            UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged;
        }

        public Binding(string path) : this()
        {
            Path = path != null ? new PropertyPath(path) : null;
        }

        public new object Source
        {
            get => base.Source;
            set
            {
                base.Source = value;

                // Run Path.set if value is null or empty
                if (string.IsNullOrEmpty(Path?.Path))
                    Path = null;
            }
        }

        public new PropertyPath Path
        {
            get => base.Path;
            set
            {
                // When source is Reactive and Path is empty, then set Path=Value and Mode
                if (Source?.GetType().IsGenericType ?? false)
                {
                    if (Source.GetType().GetGenericTypeDefinition() == typeof(ReadonlyReactiveProperty<>))
                    {
                        if (string.IsNullOrEmpty(value?.Path))
                        {
                            base.Path = new PropertyPath("Value");
                            Mode = System.Windows.Data.BindingMode.OneWay;
                            return;
                        }
                    }
                    else if (Source.GetType().GetGenericTypeDefinition() == typeof(Property<>))
                    {
                        if (string.IsNullOrEmpty(value?.Path))
                        {
                            base.Path = new PropertyPath("Value");
                            Mode = System.Windows.Data.BindingMode.TwoWay;
                            return;
                        }
                    }
                }

                // If Path is not set, then Path=null

                string propertyName = value?.Path;

                if (string.IsNullOrEmpty(propertyName))
                    base.Path = null;

                if (Source != null)
                {
                    PropertyInfo propertyInfo;
                    try
                    {
                        propertyInfo = Source.GetType().GetProperty(value.Path);
                    }
                    catch (AmbiguousMatchException)
                    {
                        return;
                    }

                    // If Path selects Reactive object, then adds .Value to end and set Mode

                    if (propertyInfo?.PropertyType?.IsGenericType ?? false)
                    {
                        if (Source.GetType().GetGenericTypeDefinition() == typeof(ReadonlyReactiveProperty<>))
                        {
                            base.Path = new PropertyPath(value.Path + ".Value");
                            Mode = System.Windows.Data.BindingMode.OneWay;
                        }
                        else if (Source.GetType().GetGenericTypeDefinition() == typeof(Property<>))
                        {
                            base.Path = new PropertyPath(value.Path + ".Value");
                            Mode = System.Windows.Data.BindingMode.TwoWay;
                        }
                    }
                }
            }
        }
    }
}
