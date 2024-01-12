namespace ConsoleContainer.Wpf.Serialization
{
    internal interface ISerializer
    {
        byte[] Serialize<T>(T value);
        T? Deserialize<T>(byte[] data);
    }
}
