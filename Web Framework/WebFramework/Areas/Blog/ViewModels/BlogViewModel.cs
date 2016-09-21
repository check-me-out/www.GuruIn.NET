using System;
using System.Collections.Generic;
using WebFramework.Models.Blog;

namespace WebFramework.Areas.Blog.ViewModels
{
    public class BlogViewModel
    {
        public string AllPostsApiEndpoint { get; set; }

        public string SelectedPostApiEndpoint { get; set; }

        public string Category { get; set; }
        public string UrlSlug { get; set; }
        public string Tag { get; set; }
        public string Archive { get; set; }
        public string SearchTerm { get; set; }
        public int? CurrPage { get; set; }
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }

        public int TotalCount { get; set; }
        public List<Post> AllPosts { get; set; }

        public int? PrevId { get; set; }
        public string PrevSlug { get; set; }
        public int? NextId { get; set; }
        public string NextSlug { get; set; }

        public Post SelectedPost { get; set; }
        public bool DisableComments { get; set; }

        public static string ReplaceString(string str, string oldValue, string newValue, StringComparison comparison)
        {
            var retainCase = false;
            if (comparison == StringComparison.CurrentCultureIgnoreCase || comparison == StringComparison.InvariantCultureIgnoreCase || comparison == StringComparison.OrdinalIgnoreCase)
            {
                retainCase = true;
            }

            var sb = new System.Text.StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));

                if (retainCase)
                {
                    int idx = str.IndexOf(oldValue, index, comparison);
                    if (idx > -1) newValue = newValue.Replace(oldValue, str.Substring(idx, oldValue.Length));
                    sb.Append(newValue);
                }
                else
                {
                    sb.Append(newValue);
                }

                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }
    }
}