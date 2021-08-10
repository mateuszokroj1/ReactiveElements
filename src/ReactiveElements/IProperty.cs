namespace ReactiveElements
{
    public interface IProperty<T> : IReadonlyProperty<T>
    {
        new T Value { get; set; }

        void SetValue(T value);

        void SetValue(object value);
    }
}
