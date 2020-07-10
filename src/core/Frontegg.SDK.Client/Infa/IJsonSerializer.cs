namespace Frontegg.SDK.Client.Infa
{
    internal interface IJsonSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }
}