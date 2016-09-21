using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.Blog
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string UrlSlug { get; set; }

        public bool Published { get; set; }

        public DateTime PostedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Category Category { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Post()
        {
            Tags = new HashSet<Tag>();
        }
    }
}
