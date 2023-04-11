namespace WorkQR.Application
{
    public class SelectVM<T>
    {
        public string Label { get; set; } = "";
        public T? Value { get; set; }
    }
}
