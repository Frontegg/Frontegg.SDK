using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Audits
{
    public interface IAuditsClient
    {
        Task SendAudit(string tenantId, object audit);
    }
}