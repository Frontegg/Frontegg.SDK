namespace Frontegg.SDK.Client.Extensions
{
    internal static class StringExtensions
    {
        public static string ChopTail(this string target, int count)
        {
            if (count >= target.Length)
                return string.Empty;

            return target.Remove(target.Length - count);
        }

        
    }
}