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
using Complete.Service.EntityServices;

namespace Complete.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private SupplierService supplierService = new SupplierService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Supplier>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        public ActionResult Index()
        {
            return View(supplierService.Load(x => x.DateDeleted == null).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierService.LoadByID(Convert.ToInt32(id));
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierId,SupplierName,DateCreated,DateModfied,DateDeleted")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (!supplierService.CheckDuplicate(supplier))
                {
                    supplier.DateCreated = DateTime.Now;
                    supplierService.Add(supplier);
                    supplierService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Supplier already exists";
            }
            return View(supplier);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierService.LoadByID(Convert.ToInt32(id));
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierId,SupplierName,DateCreated,DateModfied,DateDeleted")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (supplierService.UpdateValidate(supplier))
                {
                    supplier.DateModfied = DateTime.Now;
                    supplierService.Update(supplier);
                    supplierService.Save();  
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Supplier with same name already exists";
            }
            return View(supplier);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierService.LoadByID(Convert.ToInt32(id));
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = supplierService.LoadByID(id);
            if (supplierService.ValidateDelete(supplier))
            {
                supplier.DateDeleted = DateTime.Now;
                supplierService.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Supplier has Products and cannot be deleted";
            return View(supplier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                supplierService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
