using log4net;
using Newtonsoft.Json.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebFramework.Areas.Blog.ViewModels;
using WebFramework.Controllers;
using WebFramework.Helpers;
using WebFramework.Models.Blog;
using WebFramework.Persistence.Blog;

namespace WebFramework.Areas.Blog.Controllers
{
    //[RoutePrefix("api/Blog")]
    public class BlogController : ApiController
    {
        public const string AllPostsRoute = "AllPostsRoute";
        public const string SelectedPostRoute = "SelectedPostRoute";

        private readonly ILog _log;

        private readonly IBlogDbContext _db;

        private static List<Archive> _archives;

        public BlogController([Named(Constants.ServerLoggerName)] ILog log, IBlogDbContext db)
        {
            _log = log;
            _db = db;
        }

        public static string GetEndpointUrl(HttpRequestBase request, string routeName)
        {
            if (request == null)
            {
                return null;
            }

            var endpoint = new System.Web.Mvc.UrlHelper(request.RequestContext).RouteUrl(routeName, new { httproute = true });
            if (string.IsNullOrWhiteSpace(endpoint)) endpoint = "/api/Blog";
            return endpoint;
        }

        public static string GetBaseUrl()
        {
            return "/Blog/api";
        }

        [HttpGet]
        public BlogViewModel GetAllPosts(string slug = null, string category = null, string tag = null, int? cur = null, string archive = null, string searchTerm = null)
        {
            var viewModel = new BlogViewModel();

            var pageSize = ConfigManager.Get<int>(Constants.ConfigKey.Blog_AllPosts_PageSize);

            IEnumerable<Post> list = _db.Posts
                          .Include("Category")
                          .Include("Tags")
                          .Where(p => category == null || category == "" || p.Category.Name.Equals(category, System.StringComparison.InvariantCultureIgnoreCase))
                          .Where(p => tag == null || tag == "" || p.Tags.Any(a => a.Name.Equals(tag, System.StringComparison.InvariantCultureIgnoreCase)))
                          .Where(p => searchTerm == null || searchTerm == "" || p.Title.Contains(searchTerm) || p.ShortDescription.Contains(searchTerm))
                          .OrderByDescending(o => o.PostedOn)
                          .ToList();

            list = list.Where(p => archive == null || archive == "" || p.PostedOn.ToString("yyyy - MMM").Equals(archive, System.StringComparison.InvariantCultureIgnoreCase)).ToList();

            viewModel.TotalCount = list.Count();

            if (!string.IsNullOrWhiteSpace(category)) viewModel.Category = category;

            if (!string.IsNullOrWhiteSpace(archive)) viewModel.Archive = archive;

            if (!string.IsNullOrWhiteSpace(slug)) viewModel.UrlSlug = slug;

            if (!string.IsNullOrWhiteSpace(tag)) viewModel.Tag = tag;

            if (!string.IsNullOrWhiteSpace(searchTerm)) viewModel.SearchTerm = searchTerm;

            if (cur.HasValue) viewModel.CurrPage = cur;
            else cur = 0;

            var totalAvailable = list.Count();
            if (totalAvailable > (cur.Value + 1) * pageSize)
            {
                viewModel.NextPage = cur.Value + 1;
            }

            if (cur.Value > 0 && totalAvailable > 0)
            {
                viewModel.PrevPage = cur.Value - 1;
            }

            viewModel.AllPosts = list.Skip(cur.Value * pageSize).Take(pageSize).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                viewModel.AllPosts.ForEach(p => p.Title = BlogViewModel.ReplaceString(p.Title, searchTerm, "<span class=\"highlight-search-term\">" + searchTerm + "</span>", StringComparison.CurrentCultureIgnoreCase));
                viewModel.AllPosts.ForEach(p => p.ShortDescription = BlogViewModel.ReplaceString(p.ShortDescription, searchTerm, "<span class=\"highlight-search-term\">" + searchTerm + "</span>", StringComparison.CurrentCultureIgnoreCase));
            }

            return viewModel;
        }
/*
        [System.Web.Http.Route("GetPost/{id}/{slug}", Name = SelectedPostRoute)]
        public BlogViewModel GetPost(int id, string slug, string category, string tag, int? cur, string archive, string searchTerm)
        {
            var viewModel = new BlogViewModel();

            if (!string.IsNullOrWhiteSpace(category)) viewModel.Category = category;

            if (!string.IsNullOrWhiteSpace(archive)) viewModel.Archive = archive;

            if (!string.IsNullOrWhiteSpace(slug)) viewModel.UrlSlug = slug;

            if (!string.IsNullOrWhiteSpace(tag)) viewModel.Tag = tag;

            if (!string.IsNullOrWhiteSpace(searchTerm)) viewModel.SearchTerm = searchTerm;

            if (cur.HasValue) viewModel.CurrPage = cur;

            var post = _db.Posts.Find(id);
            if (post == null)
            {
                throw new HttpException(404, "The blog post you are looking for is not available.");
            }

            _db.Entry(post).Reference("Category").Load();
            _db.Entry(post).Collection("Tags").Load();
            _db.Entry(post).Collection("Comments").Load();

            var prev = _db.Posts.Where(p => p.PostedOn < post.PostedOn).OrderByDescending(o => o.PostedOn).FirstOrDefault();
            var next = _db.Posts.Where(p => p.PostedOn > post.PostedOn).OrderBy(o => o.PostedOn).FirstOrDefault();

            if (prev != null)
            {
                viewModel.PrevId = prev.Id;
                viewModel.PrevSlug = prev.UrlSlug;
            }

            if (next != null)
            {
                viewModel.NextId = next.Id;
                viewModel.NextSlug = next.UrlSlug;
            }

            viewModel.SelectedPost = post;

            if (post.Comments != null && post.Comments.Count > ConfigManager.Get<int>(Constants.ConfigKey.Blog_Posts_MaxComments))
            {
                viewModel.DisableComments = true;
            }

            return viewModel;
        }
*/
        // GET: Blog/GetArchives
        public List<Archive> GetArchives()
        {
            if (_archives == null || _archives.Count == 0)
            {
                var list = _db.Posts.OrderByDescending(o => o.PostedOn).ToList();
                var dict = list.Select(s => s.PostedOn.ToString("yyyy - MMM")).GroupBy(g => g).ToDictionary(d => d.Key, d => d.Count());

                if (_archives == null || _archives.Count == 0)
                {
                    _archives = dict.Select(d => new Archive { Period = d.Key, Count = d.Value }).ToList();
                }
            }

            return _archives;
        }

        // GET: Blog/GetCategories
        public List<Category> GetCategories()
        {
            var list = _db.Categories.OrderBy(o => o.Name).ToList();

            return list;
        }

        // GET: Blog/GetTags
        public List<Tag> GetTags()
        {
            var list = _db.Tags.GroupBy(g => g.Name).Select(s => s.FirstOrDefault()).OrderBy(o => o.Name).ToList();

            return list;
        }
    }
}