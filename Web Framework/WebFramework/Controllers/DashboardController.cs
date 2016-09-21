using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Ninject;
using System.Configuration;
using WebFramework.Helpers;
using WebFramework.Models.Products;
using WebFramework.Persistence.Products;

namespace WebFramework.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILog _log;

        private readonly IProductsDbContext _db;

        public DashboardController([Named(Constants.ServerLoggerName)] ILog log, IProductsDbContext db)
            : base(log)
        {
            _log = log;
            _db = db;
        }

        public ActionResult ComingUp()
        {
            return View("~/Views/Home/About.cshtml");
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult Remove()
        {
            throw new NotImplementedException("This message is from the exception. This exception has an inner exception.",
                new Exception("This message is from the inner exception. This inner exception has a further inner exception.",
                    new Exception("This message is from the second-level inner exception. This inner exception has no further inner exceptions.")));
        }

        public ActionResult List()
        {
            var list = _db.Items.ToList();
            ViewBag.ListCount = list.Count;

            return View(list);
        }

        public ActionResult Details(int id = 0)
        {
            Item model = _db.Items.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var list = _db.Items.ToList();
            ViewBag.ListCount = list.Count;

            return View(model);
        }

        public ActionResult Create()
        {
            var list = _db.Items.ToList();
            ViewBag.ListCount = list.Count;

            return View();
        }

        [HttpPost]
        public ActionResult Create(Item model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedOn = DateTime.Now;

                _db.Items.Add(model);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Item model = _db.Items.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var list = _db.Items.ToList();
            ViewBag.ListCount = list.Count;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Item model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id = 0)
        {
            Item model = _db.Items.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var list = _db.Items.ToList();
            ViewBag.ListCount = list.Count;

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item model = _db.Items.Find(id);
            _db.Items.Remove(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
