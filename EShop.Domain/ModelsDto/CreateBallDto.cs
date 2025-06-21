namespace EShop.Domain.ModelsDto
{
    public class CreateBallDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; }
    }
}
