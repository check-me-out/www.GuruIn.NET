using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Products
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Category Category { get; set; }
    }
}
