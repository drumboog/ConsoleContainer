namespace ConsoleContainer.Repositories.Serialization
{
    internal interface IBinarySerializer
    {
        byte[] Serialize<T>(T value);
        T? Deserialize<T>(byte[] data);
    }
}
