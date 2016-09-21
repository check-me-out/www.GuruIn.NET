namespace WebFramework.Persistence.Migrations.Products
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebFramework.Models.Products;
    using WebFramework.Persistence.Products;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ProductsDbContext context)
        {
            var clothingSubCategories = new List<SubCategory>
                {
                    new SubCategory { Id = 1, Name = "Suits Wear", Description = "Suits Wear" },
                    new SubCategory { Id = 2, Name = "Formals Wear", Description = "Formals Wear" },
                    new SubCategory { Id = 3, Name = "Casuals Wear", Description = "Casuals Wear" },
                    new SubCategory { Id = 4, Name = "Sports Wear", Description = "Sports Wear" },
                };
            context.Categories.AddOrUpdate(new Category { Id = 1, Name = "Clothing", Description = "Clothing", SubCategories = clothingSubCategories });

            var decorSubCategories = new List<SubCategory>
                {
                    new SubCategory { Id = 5, Name = "Living Room", Description = "Living Room" },
                    new SubCategory { Id = 6, Name = "Bedroom", Description = "Bedroom" },
                    new SubCategory { Id = 7, Name = "Kitchen & Dining", Description = "Kitchen & Dining" },
                    new SubCategory { Id = 8, Name = "Accessories", Description = "Accessories" },
                };
            context.Categories.AddOrUpdate(new Category { Id = 2, Name = "Decor", Description = "Home Decorations", SubCategories = decorSubCategories });

            context.Colors.AddOrUpdate(new Color { Id = 1, Name = "White", Description = "White", ColorCode = "#fff" });
            context.Colors.AddOrUpdate(new Color { Id = 2, Name = "Black", Description = "Black", ColorCode = "#000" });
            context.Colors.AddOrUpdate(new Color { Id = 3, Name = "Red", Description = "Red", ColorCode = "red" });
            context.Colors.AddOrUpdate(new Color { Id = 4, Name = "Green", Description = "Green", ColorCode = "green" });
            context.Colors.AddOrUpdate(new Color { Id = 5, Name = "Blue", Description = "Blue", ColorCode = "blue" });
            context.Colors.AddOrUpdate(new Color { Id = 6, Name = "Yellow", Description = "Yellow", ColorCode = "yellow" });
            context.Colors.AddOrUpdate(new Color { Id = 7, Name = "Orange", Description = "Orange", ColorCode = "orange" });
            context.Colors.AddOrUpdate(new Color { Id = 8, Name = "Purple", Description = "Purple", ColorCode = "purple" });

            context.Sizes.AddOrUpdate(new Size { Id = 1, Name = "XS", Description = "Extra Small" });
            context.Sizes.AddOrUpdate(new Size { Id = 2, Name = "S", Description = "Small" });
            context.Sizes.AddOrUpdate(new Size { Id = 3, Name = "M", Description = "Medium" });
            context.Sizes.AddOrUpdate(new Size { Id = 4, Name = "L", Description = "Large" });
            context.Sizes.AddOrUpdate(new Size { Id = 5, Name = "XL", Description = "Extra Large" });
            context.Sizes.AddOrUpdate(new Size { Id = 6, Name = "XXL", Description = "Double Extra Large" });
            context.Sizes.AddOrUpdate(new Size { Id = 7, Name = "XXXL", Description = "Triple Extra Large" });

            context.SaveChanges();
        }
    }
}
