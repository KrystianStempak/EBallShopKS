namespace EBallShop.Models
{
    public class Ball
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
