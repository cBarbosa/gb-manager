using System;

namespace gb_manager.Domain.Models
{
    public class Preference
    {
        public int? Id { get; set; }
        public string RecordId { get; set; }
        public int? InstallmentId { get; set; }
        public string InitPoint { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool Active { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}