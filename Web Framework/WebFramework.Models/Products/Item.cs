using System;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Products
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public SubCategory SubCategory { get; set; }

        public Color Color { get; set; }

        public Size Size { get; set; }

        public Dimension Dimension { get; set; }

        [Display(Name = "Was")]
        public decimal? CurrentPrice { get; set; }

        [Required]
        [Display(Name = "Now")]
        public decimal? NewPrice { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool Deleted { get; set; }

        public string ImageUrl { get; set; }

        public string BuyUrl { get; set; }
    }
}
