using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Blog
{
    public class Archive
    {
        public string Period { get; set; }

        public int Count { get; set; }
    }
}
