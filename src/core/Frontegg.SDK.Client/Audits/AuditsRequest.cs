using System;
using System.Collections.Generic;

namespace Frontegg.SDK.Client.Audits
{
    public class AuditsRequest
    {
        public string TenantId { get; }
        public string Filter { get; set; }
        public string SortBy { get; set; }
        public SortDirection Sort { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public IEnumerable<string> Filters { get; set; }
        
        public AuditsRequest(string tenantId)
        {
            TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
        }
    }
}