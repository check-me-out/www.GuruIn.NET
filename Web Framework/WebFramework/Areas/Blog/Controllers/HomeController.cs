using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFramework.Areas.Blog.ViewModels;
using WebFramework.Controllers;
using WebFramework.Helpers;
using WebFramework.Models.Blog;
using WebFramework.Persistence.Blog;

namespace WebFramework.Areas.Blog.Controllers
{
    public class HomeController : BaseController
    {
        private const string DeleteCommentCode = "l8axRk1HCKSqaElTgoy4dAKJ7RzZIbhFjrK";

        private readonly ILog _log;

        private readonly IBlogDbContext _db;

        public HomeController([Named(Constants.ServerLoggerName)] ILog log, IBlogDbContext db)
        {
            _log = log;
            _db = db;
        }

        public ActionResult AllPosts()
        {
            var model = new BlogViewModel 
                                {
                                    AllPostsApiEndpoint = BlogController.GetBaseUrl(),
                                };

            return View(model);
        }

        public ActionResult Post(int id, string slug, string category, string tag, int? cur, string archive, string searchTerm)
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

            return View(viewModel);
        }

        public ActionResult DeleteComment(int id, string slug, int postId)
        {
            if (DeleteCommentCode.Equals(slug, StringComparison.CurrentCultureIgnoreCase))
            {
                var post = _db.Posts.Find(postId);
                if (post == null)
                {
                    return RedirectToAction("AllPosts");
                }

                _db.Entry(post).Collection("Comments").Load();
                if (post.Comments == null)
                {
                    return RedirectToAction("AllPosts");
                }

                var comment = post.Comments.FirstOrDefault(f => f.Id == id);
                if (comment != null)
                {
                    post.Comments.Remove(comment);
                    _db.SaveChanges();
                    return RedirectToAction("Post", new { id = postId });
                }
            }

            return RedirectToAction("AllPosts");
        }

        public JsonResult PerformCommentModeration(string comment)
        {
            var result = string.Empty;
            var words = (comment ?? string.Empty).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var badword = _db.BadWords.FirstOrDefault(f => words.Any(a => a.Equals(f.Keyword, StringComparison.CurrentCultureIgnoreCase)));
            if (badword != null)
            {
                result = string.Format(ConfigManager.Get<string>(Constants.ConfigKey.Blog_Post_Comment_Badwords_ErrorMsg), badword.Keyword);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var noOfComments = GetNoOfComments();
                if (noOfComments >= 5)
                {
                    DateTime? nextCommentAt = GetNextCommentAt();
                    var laterOn = nextCommentAt.HasValue ? " after " + nextCommentAt.Value.ToString() : string.Empty;
                    ModelState.AddModelError("", "Sorry, you are commenting too much, too soon. Take a break and comeback" + laterOn + ".");
                }
            }

            if (ModelState.IsValid)
            {
                model.NewComment.Content = model.NewComment.Content.Replace("https://", string.Empty);
                model.NewComment.Content = model.NewComment.Content.Replace("http://", string.Empty);
                model.NewComment.Content = model.NewComment.Content.Replace("www.", string.Empty);
                model.NewComment.Content = model.NewComment.Content.Replace("\r\n", "<br />");
                model.NewComment.Content = model.NewComment.Content.Replace(System.Environment.NewLine, "<br />");

                var post = _db.Posts.Find(model.PostId);
                if (post == null)
                {
                    throw new HttpException(404, "The blog post you are looking for is not available.");
                }

                _db.Entry(post).Collection("Comments").Load();
                if (post.Comments == null) post.Comments = new List<Comment>();

                model.NewComment.CommentedOn = DateTime.Now;

                post.Comments.Add(model.NewComment);

                _db.SaveChanges();

                var postId = model.PostId;
                var disableNewComment = model.DisableNewComment;
                var newCommentCopy = model.NewComment;

                ModelState.Clear();
                model = new CommentViewModel()
                {
                    PostId = postId,
                    AllComments = post.Comments,
                    NewComment = new Comment(),
                    DisableNewComment = disableNewComment
                };

                IncrementNoOfComments();

                NotifyNewComment(newCommentCopy, postId);
            }
            else
            {
                var post = _db.Posts.Find(model.PostId);
                if (post == null)
                {
                    throw new HttpException(404, "The blog post you are looking for is not available.");
                }

                _db.Entry(post).Collection("Comments").Load();
                if (post.Comments == null) post.Comments = new List<Comment>();

                model = new CommentViewModel() { AllComments = post.Comments, NewComment = model.NewComment };
            }

            return PartialView("_commentsPartialView", model);
        }

        private bool NotifyNewComment(Comment newComment, int postId)
        {
            try
            {
                var domain = Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, string.Empty);
                var postLink = domain + "/Blog/Post/" + postId;
                var commentLink = domain + "/Blog/Post/" + postId + "#comment-" + newComment.Id;
                var deleteLink = domain + "/Blog/DeleteComment/" + newComment.Id + "/" + DeleteCommentCode + "?postId=" + postId;

                var title = "New Comment on GuruIn.NET Blog";
                var subject = "Review a new comment on GuruIn.NET Blog";

                var template = EmailEngine.GetTemplate("new-comment-template");
                var body = template.Replace("*|MC:TITLE|*", title)
                                   .Replace("*|MC:SUBJECT|*", subject)
                                   .Replace("*|MC:EMAILTOBROWSERLINK|*", "http://www.GuruIn.NET")
                                   .Replace("{0}", newComment.Name)
                                   .Replace("{1}", newComment.Email)
                                   .Replace("{2}", newComment.Content.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>"))
                                   .Replace("{3}", postLink)
                                   .Replace("{4}", commentLink)
                                   .Replace("{5}", deleteLink)
                                   .Replace("*|CURRENT_YEAR|*", DateTime.Now.Year.ToString())
                                   .Replace("*|LIST:COMPANY|*", "GuruIn.NET");

                var fromTo = ConfigManager.Get<string>("SmtpUserName");

                var result = EmailEngine.SendEmail(fromTo, "GuruIn.NET", fromTo, subject, subject, body);
                return result;
            }
            catch (Exception ex)
            {
                _log.Error("BlogsController - NotifyNewComment", ex);
                return false;
            }
        }

        private int GetNoOfComments()
        {
            var cookie = Request.Cookies["NO_OF_COMMENTS"];
            if (cookie != null)
            {
                var noOfComments = 0;
                var value = cookie.Value ?? string.Empty;
                if (int.TryParse(value, out noOfComments))
                {
                    return noOfComments;
                }
            }

            return 0;
        }

        private DateTime? GetNextCommentAt()
        {
            var cookie = Request.Cookies["NEXT_COMMENT_AT"];
            if (cookie != null)
            {
                DateTime nextCommentAt;
                var value = cookie.Value ?? string.Empty;
                if (DateTime.TryParse(value, out nextCommentAt))
                {
                    return nextCommentAt;
                }
            }

            return null;
        }

        private void IncrementNoOfComments()
        {
            var noOfComments = GetNoOfComments();
            var nextCommentAt = DateTime.Now.Add(TimeSpan.FromMinutes(20));

            var cookie = new HttpCookie("NO_OF_COMMENTS", (++noOfComments).ToString());
            cookie.Expires = nextCommentAt;
            Response.Cookies.Add(cookie);

            var expiry = new HttpCookie("NEXT_COMMENT_AT", nextCommentAt.ToString());
            expiry.Expires = nextCommentAt;
            Response.Cookies.Add(expiry);
        }
    }
}