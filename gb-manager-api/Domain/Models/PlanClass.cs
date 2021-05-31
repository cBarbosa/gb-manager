namespace gb_manager.Domain.Models
{
    public class PlanClass
    {
        public int? Id { get; set; }
        public int? PlanId { get; set; }
        public int? ClassId { get; set; }
        public decimal? Discount { get; set; }
        public float? DiscountPercent { get; set; }

        public virtual Plan Plan { get; set; }
        public virtual Class Class { get; set; }
    }
}