using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client
{
    public interface IAuditsClient
    {
        Task SendAudits(string tenantId, IEnumerable<object> audits);
        //Task<IEnumerable<object>> GetAudits(AuditsRequest request);
        //Task<IEnumerable<object>> GetAuditStats(string tenantId);
        //Task ExportPdf();
        //Task ExportCsv();
    }
}