using System;

namespace gb_manager.Domain.Models
{
    public class Transaction
    {
        public int? Id { get; set; }
        public int? ExternalTransaction { get; set; }
        public Guid? RecordId { get; set; }
        public Guid? ContractId { get; set; }
        public Guid? InstallmentId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Refound { get; set; }
        public decimal? Taxes { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
