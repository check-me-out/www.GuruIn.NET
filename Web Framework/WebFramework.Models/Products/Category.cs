using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Products
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
