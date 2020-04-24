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
using Complete.Service.EntityServices;

namespace Complete.Web.Controllers
{
    public class ProductsController : Controller
    {
        private ProductService productService = new ProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Product>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private CategoryService categoryService = new CategoryService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Category>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private SupplierService supplierService = new SupplierService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Supplier>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
       
        public ActionResult Index()
        {
            var products = productService.Load(x => x.DateDeleted == null).ToList();
            return View(products);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productService.LoadByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName");
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName");
            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Descriptions,CategoryId,SupplierId,DateCreated,DateModfied,DateDeleted")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (!productService.CheckDuplicate(product)) {
                    product.DateCreated = DateTime.Now;
                    productService.Add(product);
                    productService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Product with Same Category and Supplier exits";
                //db.Products.Add(product);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName",product.CategoryId);
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName",product.SupplierId);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productService.LoadByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName",product.CategoryId);
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName",product.SupplierId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Descriptions,CategoryId,SupplierId,DateCreated,DateModfied,DateDeleted")] Product product)
        {
            if (ModelState.IsValid)
            {

                if (productService.UpdateValidate(product)) {
                    product.DateModfied = DateTime.Now;
                    productService.Update(product);
                    productService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Product with Same Category and Supplier exits";
                //db.Entry(product).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName",product.CategoryId);
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName",product.SupplierId);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productService.LoadByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = productService.LoadByID(id);
            if (productService.ValidateDelete(product)) {
                product.DateDeleted = DateTime.Now;
                productService.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Product cannot be Deleted.It has billing records";
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
