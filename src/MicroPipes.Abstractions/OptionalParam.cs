namespace MicroPipes
{
    public struct OptionalParam<T>
    {
        public bool HasValue { get; }
        public T Value { get; }

        public OptionalParam(T value) : this()
        {
            Value = value;
        }

        public static implicit operator OptionalParam<T>(T value) => new OptionalParam<T>(value);

        public T NoValue(T value) => HasValue ? Value : value;
    }
}