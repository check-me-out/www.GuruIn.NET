namespace WebFramework.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    public class Dimension
    {
        [Key]
        public int Id { get; set; }

        public int? Length { get; set; }

        public int? Width { get; set; }

        public int? Depth { get; set; }

        public int? Height { get; set; }

        public int? Radius { get; set; }

        public int? Diameter { get; set; }

        public SizeUnit SizeUnit { get; set; }
    }
}
