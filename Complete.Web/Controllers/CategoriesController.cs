using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Complete.Domain;
using Complete.Repository.Repositories;
using Complete.Service.CustomServices;

namespace Complete.Web.Controllers
{
    public class CategoriesController : Controller
    {
        
        //private SmartInventoryEntities db = new SmartInventoryEntities();

        //private CategoryService categoryService;
        private CategoryService categoryService = new CategoryService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Category>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));

        public ActionResult Index()
        {
            var d = categoryService.Load(x => x.DateDeleted == null).ToList();
            return View(d);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryService.LoadByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,DateCreated,DateModfied,DateDeleted")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (!categoryService.CheckDuplicate(category))
                {
                    category.DateCreated = DateTime.Now;
                    categoryService.Add(category);
                    categoryService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Category already exists";
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryService.LoadByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,DateCreated,DateModfied,DateDeleted")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (categoryService.UpdateValidate(category)) {
                    category.DateModfied = DateTime.Now;
                    categoryService.Update(category);
                    categoryService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Category with same name already exists";
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryService.LoadByID(Convert.ToInt32(id));
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryService.LoadByID(id);
            if (categoryService.ValidateDelete(category)) {
                category.DateDeleted = DateTime.Now;
                categoryService.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Category has Products and cannot be deleted";
            return View(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                categoryService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
