namespace Frontegg.SDK.Client.Infa
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }
}