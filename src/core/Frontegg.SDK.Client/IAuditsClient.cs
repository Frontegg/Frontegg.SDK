using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client
{
    public interface IAuditsClient
    {
        // getAudits(params: { tenantId: string, filter?: string, sortBy?: string, sortDirection?: string, offset: number, count: number, filters: any })
        // getAuditsStats(params: { tenantId: string }) {
        // getAuditsMetadata()
        // setAuditsMetadata(metadata: any)
        // exportPdf(params: { tenantId: string, filter?: string, sortBy?: string, sortDirection?: string, filters: any }, properties: any[]) {
        // exportCsv(params: { tenantId: string, filter?: string, sortBy?: string, sortDirection?: string, filters: any }, properties: any[]) {
        
        Task SendAudits(IEnumerable<object> audits);
        //Task<IEnumerable<object>> GetAudits(AuditsRequest request);
        //Task<IEnumerable<object>> GetAuditStats(string tenantId);
        //Task ExportPdf();
        //Task ExportCsv();
    }
}