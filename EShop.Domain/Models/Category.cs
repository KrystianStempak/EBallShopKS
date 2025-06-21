using AutoMapper;

namespace EShop.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Ball Ball { get; set; }
    }
}
