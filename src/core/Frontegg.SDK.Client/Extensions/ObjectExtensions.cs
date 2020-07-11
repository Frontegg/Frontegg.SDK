namespace Frontegg.SDK.Client.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsString(this object target)
        {
            return target is string;
        }
    }
}