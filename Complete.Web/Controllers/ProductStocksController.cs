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
    public class ProductStocksController : Controller
    {
        private ProductStockService  productStockService= new ProductStockService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<ProductStock>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private ProductService  productService= new ProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Product>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));

        public ActionResult Index()
        {
            var productStocks = productStockService.Load(x=>x.DateDeleted==null);
            return View(productStocks.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock productStock = productStockService.LoadByID(id);
            if (productStock == null)
            {
                return HttpNotFound();
            }
            return View(productStock);
        }

        public ActionResult Create()
        {
            var existingProductIds = productStockService.Load(x => x.DateDeleted == null).Select(x => x.ProductId).ToList();
            ViewBag.ProductId = new SelectList(productService.Load(x=>x.DateDeleted==null&&!existingProductIds.Contains(x.ProductId)), "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductStockId,ProductId,MinimumStockQuantity,DateCreated,DateModfied,DateDeleted")] ProductStock productStock)
        {
            if (ModelState.IsValid)
            {
                if (!productStockService.CheckDuplicate(productStock)) {
                    productStock.DateCreated = DateTime.Now;
                    productStockService.Add(productStock);
                    productStockService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Stock already exists for given product";
            }

            ViewBag.ProductId = new SelectList(productService.Load(x=>x.DateDeleted==null), "ProductId", "ProductName", productStock.ProductId);
            return View(productStock);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock productStock = productStockService.LoadByID(id);
            if (productStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductName = productStock.Product.ProductName;
            //ViewBag.ProductId = new SelectList(productService.Load(x=>x.DateDeleted==null), "ProductId", "ProductName", productStock.ProductId);
            return View(productStock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductStockId,ProductId,MinimumStockQuantity,DateCreated,DateModfied,DateDeleted")] ProductStock productStock)
        {
            if (ModelState.IsValid)
            {
                if (productStockService.UpdateValidate(productStock)) {
                    productStock.DateModfied = DateTime.Now;
                    productStockService.Update(productStock);
                    productStockService.Save();
                    return RedirectToAction("Index");
                }
                Product product = productService.LoadByID(productStock.ProductId);
                ViewBag.ProductName = product.ProductName;
                ViewBag.Error = "Stock cannot be less than zero";
            }
            //ViewBag.ProductId = new SelectList(productService.Load(x=>x.DateDeleted==null), "ProductId", "ProductName", productStock.ProductId);
            return View(productStock);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock productStock = productStockService.LoadByID(id);
            if (productStock == null)
            {
                return HttpNotFound();
            }
            return View(productStock);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductStock productStock = productStockService.LoadByID(id);
            productStock.DateDeleted = DateTime.Now;
            productStockService.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productStockService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
