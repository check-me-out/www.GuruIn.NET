using System;
using System.ComponentModel.DataAnnotations;
using WebFramework.Models.Helpers;

namespace WebFramework.Models.Blog
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required, CheckMyName]
        public string Name { get; set; }

        [EmailAddress]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Please enter a valid email address!")]
        public string Email { get; set; }

        [Required,
        Display(Name = "Comment"),
        StringLength(3000, ErrorMessage = "Comment too long, consider breaking it up.")]
        public string Content { get; set; }

        public DateTime CommentedOn { get; set; }

        public bool NotifyOnComments { get; set; }

        public bool NotifyOnPosts { get; set; }
    }
}
