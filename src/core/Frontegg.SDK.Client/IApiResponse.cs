namespace Frontegg.SDK.Client
{
    internal interface IApiResponse<out T>
    {
        T Body { get; }
    }
}