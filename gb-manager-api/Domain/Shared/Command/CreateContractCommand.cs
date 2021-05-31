using System;

namespace gb_manager.Domain.Shared.Command
{
    public class CreateContractCommand
    {
        public int? Id { get; set; }
        public int? PlanId { get; set; }
        public int? PersonId { get; set; }
        public Guid? RecordId { get; set; }
        public int? Installments { get; set; }
        public decimal? Amount { get; set; }
        public int? BillingDay { get; set; }
        public DateTime? Starts { get; set; }
        public DateTime? Ends { get; set; }
        public bool Active { get; set; }
    }
}