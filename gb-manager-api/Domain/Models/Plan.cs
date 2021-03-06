using System;
using System.Collections.Generic;

namespace gb_manager.Domain.Models
{
    public class Plan
    {
        public int? Id { get; set; }
        public Guid? RecordId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
        public float? DiscountPercent { get; set; }
        public bool Active { get; set; }

        public virtual IEnumerable<Class> Classes { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}