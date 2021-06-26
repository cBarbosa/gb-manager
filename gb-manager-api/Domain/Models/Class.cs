using System;

namespace gb_manager.Domain.Models
{
    public class Class
    {
        public int? Id { get; set; }
        public Guid? RecordId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }
        public bool Active { get; set; }
        public virtual Grade Grade { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}