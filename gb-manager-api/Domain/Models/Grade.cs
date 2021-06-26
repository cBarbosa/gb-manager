using System;

namespace gb_manager.Domain.Models
{
    public class Grade
    {
        public int? Id { get; set; }
        public string WeekDay { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
    }
}