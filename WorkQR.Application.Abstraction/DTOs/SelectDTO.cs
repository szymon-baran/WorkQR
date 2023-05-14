namespace WorkQR.Application
{
    public class SelectDTO<T>
    {
        public string Label { get; set; } = "";
        public T? Value { get; set; }
    }
}
