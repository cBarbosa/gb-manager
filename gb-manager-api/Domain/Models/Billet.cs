using System;

namespace gb_manager.Domain.Models
{
    public class Billet
    {
        public int? Id { get; set; }
        public Guid? RecordId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Receipt { get; set; }
        public string Transaction { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}