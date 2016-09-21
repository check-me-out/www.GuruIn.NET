using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Products
{
    public class Size
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
