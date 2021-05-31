using System;

namespace gb_manager.Domain.Models
{
    public class ContractPerson
    {
        public int? Id { get; set; }
        public int? ContractId { get; set; }
        public int? PersonId { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }


        public virtual Contract Contract { get; set; }
        public virtual Person Person { get; set; }
    }
}