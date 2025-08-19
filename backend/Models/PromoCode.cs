namespace backend.Models
{
    public class PromoCode
    {
        public string PromoCode { get; set; } = string.Empty;
        public string Variance { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime GeneratedOn { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
