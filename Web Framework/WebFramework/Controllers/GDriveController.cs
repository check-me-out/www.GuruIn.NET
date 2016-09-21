namespace WebFramework.Controllers
{
    using log4net;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using WebFramework.App_Start;
    using WebFramework.Helpers;
    using WebFramework.Persistence.GDrive;

    [SecurityCode]
    public class GDriveController : BaseController
    {
        private readonly ILog _log;

        private readonly IGDriveDbContext _db;

        public GDriveController([Named(WebFramework.Helpers.Constants.ServerLoggerName)] ILog log, IGDriveDbContext db)
            : base(log)
        {
            _log = log;
            _db = db;
        }

        public ActionResult Index()
        {
            ViewBag.MaxNoOfFiles = ConfigManager.Get<string>("MaxNoOfFiles");
            ViewBag.MaxFileSize = ConfigManager.Get<decimal>("MaxFileSizeInMB");
            ViewBag.DriveFileFormats = ConfigManager.Get<string>("DriveFileFormats");

            return View();
        }

        [HttpPost]
        public ActionResult Index(System.Web.HttpPostedFileBase file)
        {
            var maxNoOfFiles = ConfigManager.Get<int>("MaxNoOfFiles");
            var maxFileSize = ConfigManager.Get<decimal>("MaxFileSizeInMB");
            var driveFileFormats = ConfigManager.Get<string>("DriveFileFormats");
            ViewBag.MaxNoOfFiles = maxNoOfFiles;
            ViewBag.MaxFileSize = maxFileSize;
            ViewBag.DriveFileFormats = driveFileFormats;

            var msg = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    var formats = driveFileFormats.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var ext = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);

                    var identity = (ClaimsIdentity)User.Identity;
                    var claimsUserData = identity.Claims.FirstOrDefault(a => a.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata");
                    var superUser = false;
                    if (claimsUserData != null && claimsUserData.Value == ConfigManager.Get<string>("SecretSecurityCode"))
                    {
                        superUser = true;
                    }

                    var fileSize = ((decimal)file.ContentLength) / 1024.0m / 1024.0m; // in MB.
                    if (!superUser && fileSize > maxFileSize)
                    {
                        msg = "Selected file (" + fileSize.ToString("0.00") + "MB) is exceeding the limit.";
                    }
                    else if (!superUser && !formats.Any(a => a.Trim().Equals(ext, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        msg = ext.ToUpper() + " file format is not accepted.";
                    }
                    else if (!superUser && GetNoOfFiles() > maxNoOfFiles)
                    {
                        msg = "You have reached your quota of files.";
                    }
                    else if (_db.Files.FirstOrDefault(f => f.FileName == file.FileName) != null)
                    {
                        msg = "File with same name already exists.";
                    }
                    else if (claimsUserData == null)
                    {
                        msg = "Unable to read your security code. Logout and log back in please.";
                    }
                    else
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        }
                        if (fileData == null || fileData.Length == 0)
                        {
                            return RedirectToAction("Index");
                        }

                        _db.Files.Add(new Models.GDrive.FileContent
                                                {
                                                    SecurityCode = claimsUserData.Value,
                                                    FileName = file.FileName,
                                                    UploadedOn = DateTime.Now,
                                                    Content = fileData
                                                });
                        _db.SaveChanges();

                        IncrementNoOfFiles();

                        Session["USER_MESSAGE"] = "Your file has been successfully uploaded.";
                        Session["USER_MESSAGE_SEVERITY"] = "Success";
                        Session["USER_MESSAGE_TITLE"] = "File Uploaded";

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("GDrive Upload", ex);
                    msg = "Sorry, an error occured while uploading your file. Please try again.";
                }
            }
            else
            {
                msg = "You have not specified a file.";
            }

            Session["USER_MESSAGE"] = msg;
            Session["USER_MESSAGE_SEVERITY"] = "Error";

            return RedirectToAction("Index");
        }

        public ActionResult Download(string file)
        {
            var fileContent = _db.Files.FirstOrDefault(f => f.FileName == file);
            if (fileContent == null)
            {
                Session["USER_MESSAGE"] = "File not found.";
                Session["USER_MESSAGE_SEVERITY"] = "Error";
                Session["USER_MESSAGE_TITLE"] = "File not found";

                return RedirectToAction("Index");
            }

            return File(fileContent.Content, GetContentType(fileContent.FileName));
        }

        public ActionResult Delete(string file)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                var fileContent = _db.Files.FirstOrDefault(f => f.FileName == file);
                if (fileContent == null)
                {
                    Session["USER_MESSAGE"] = "File not found.";
                    Session["USER_MESSAGE_SEVERITY"] = "Error";
                    Session["USER_MESSAGE_TITLE"] = "File not found";

                    return RedirectToAction("Index");
                }

                _db.Files.Remove(fileContent);
                _db.SaveChanges();

                DecrementNoOfFiles();

                Session["USER_MESSAGE"] = "The file has been successfully deleted.";
                Session["USER_MESSAGE_SEVERITY"] = "Success";
                Session["USER_MESSAGE_TITLE"] = "File Deleted";

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public JsonResult GetFiles(string folder)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claimsUserData = identity.Claims.FirstOrDefault(a => a.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata");
            var t = _db.Files.Where(w => w.SecurityCode == claimsUserData.Value).Select(s => s.FileName).ToList();
            return new JsonResult { Data = t, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private string GetContentType(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.') + 1).ToUpper();
            switch (ext)
            {
                case "JPG":
                case "JPEG":
                    return "image/jpeg";

                case "PNG":
                    return "image/png";

                case "ZIP":
                    return "application/zip";

                default:
                    return string.Empty;
            }
        }

        private int GetNoOfFiles()
        {
            var cookie = Request.Cookies["NO_OF_FILES"];
            if (cookie != null)
            {
                var noOfFiles = 0;
                var value = cookie.Value ?? string.Empty;
                if (int.TryParse(value, out noOfFiles))
                {
                    return noOfFiles;
                }
            }

            return 0;
        }

        private void IncrementNoOfFiles()
        {
            var noOfFiles = GetNoOfFiles();
            var nextFileAt = DateTime.Now.Add(TimeSpan.FromMinutes(300));

            var cookie = new HttpCookie("NO_OF_FILES", (++noOfFiles).ToString());
            cookie.Expires = nextFileAt;
            Response.Cookies.Add(cookie);

            var expiry = new HttpCookie("NEXT_FILE_AT", nextFileAt.ToString());
            expiry.Expires = nextFileAt;
            Response.Cookies.Add(expiry);
        }

        private void DecrementNoOfFiles()
        {
            var noOfFiles = GetNoOfFiles();
            var nextFileAt = DateTime.Now.Add(TimeSpan.FromMinutes(300));

            var cookie = new HttpCookie("NO_OF_FILES", (--noOfFiles).ToString());
            cookie.Expires = nextFileAt;
            Response.Cookies.Add(cookie);

            var expiry = new HttpCookie("NEXT_FILE_AT", nextFileAt.ToString());
            expiry.Expires = nextFileAt;
            Response.Cookies.Add(expiry);
        }
    }
}