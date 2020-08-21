using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;

namespace ReactiveElements.Markup
{
    public class Binding : Windows.UI.Xaml.Data.Binding
    {
        public Binding()
        {
            UpdateSourceTrigger = Windows.UI.Xaml.Data.UpdateSourceTrigger.PropertyChanged;
        }

        public new object Source
        {
            get => base.Source;
            set
            {
                base.Source = value;
                if (string.IsNullOrEmpty(Path?.Path))
                    Path = null;
            }
        }

        public new PropertyPath Path
        {
            get => base.Path;
            set
            {
                if (Source?.GetType().IsGenericType ?? false &&
                    (Source.GetType().GetGenericTypeDefinition() == typeof(ReadonlyReactiveProperty<>) ||
                     Source.GetType().GetGenericTypeDefinition() == typeof(ReactiveProperty<>))
                )
                    if (value == null || string.IsNullOrEmpty(value.Path))
                    { 
                        base.Path = new PropertyPath("Value");
                        return;
                    }

                string propertyName = value?.Path;

                if (string.IsNullOrEmpty(propertyName))
                    base.Path = null;

                if(Source != null)
                {
                    PropertyInfo propertyInfo;
                    try
                    {
                        propertyInfo = Source.GetType().GetProperty(value.Path);
                    }
                    catch(AmbiguousMatchException)
                    {
                        return;
                    }

                    if (propertyInfo?.PropertyType?.IsGenericType ?? false &&
                        (Source.GetType().GetGenericTypeDefinition() == typeof(ReadonlyReactiveProperty<>) ||
                        Source.GetType().GetGenericTypeDefinition() == typeof(ReactiveProperty<>))
                     )
                        base.Path = new PropertyPath(value.Path + ".Value");
                }
            }
        }
    }
}
