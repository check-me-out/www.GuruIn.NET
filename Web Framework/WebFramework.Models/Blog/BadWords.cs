using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Blog
{
    public class BadWords
    {
        [Key]
        public string Keyword { get; set; }
    }
}
