using System;

namespace gb_manager.Domain.Shared.Command
{
    public class CreateContractCommand
    {
        public Guid? RecordId { get; set; }
        public Guid? PlanRecordId { get; set; }
        public Guid? PersonRecordId { get; set; }
        public int? Installments { get; set; }
        public decimal? Amount { get; set; }
        public int? BillingDay { get; set; }
        public DateTime? Starts { get; set; }
        public DateTime? Ends { get; set; }
        public bool Active { get; set; }
    }
}