using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Products
{
    public class SizeUnit
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
