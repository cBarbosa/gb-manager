using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gb_manager.Domain.Models
{
    public class Contract
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

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual Plan Plan { get; set; }
        public virtual Person Person { get; set; }
        public virtual IEnumerable<ContractPerson> Persons { get; set; }
    }
}
