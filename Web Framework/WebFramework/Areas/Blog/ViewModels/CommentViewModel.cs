using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebFramework.Helpers;
using WebFramework.Models.Blog;

namespace WebFramework.Areas.Blog.ViewModels
{
    public class CommentViewModel
    {
        public int PostId { get; set; }

        public ICollection<Comment> AllComments { get; set; }

        public Comment NewComment { get; set; }
        public bool DisableNewComment { get; set; }
    }
}