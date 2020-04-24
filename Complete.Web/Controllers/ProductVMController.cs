using Complete.Domain;
using Complete.Repository.Repositories;
using Complete.Service.CustomServices;
using Complete.Service.EntityServices;
using Complete.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Complete.Web.Controllers
{
    public class ProductVMController : Controller
    {
        private ProductService productService = new ProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Product>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private QuantityService quantityService = new QuantityService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Quantity>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private CategoryService categoryService = new CategoryService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Category>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private SupplierService supplierService = new SupplierService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Supplier>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private ProductVMService productVMService = new ProductVMService();
        public ActionResult Index()
        {
            var products = productService.Load(x => x.DateDeleted == null).ToList();
            return View(products);
        }

        // GET: ProductVM/Details/5
        public ActionResult Details(int id)
        {
            var product = productService.LoadByID(id);
            return View(product);
        }

        // GET: ProductVM/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName");
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Descriptions,CategoryId,SupplierId,Quantity,Rate")] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                if (!productVMService.CheckDuplicate(productVM.ProductName,productVM.CategoryId,productVM.SupplierId))
                {
                    Product product = new Product();
                    product.ProductName = productVM.ProductName;
                    product.CategoryId = productVM.CategoryId;
                    product.SupplierId = productVM.SupplierId;
                    product.Descriptions = productVM.Descriptions;
                    product.DateCreated = DateTime.Now;
                    productService.Add(product);
                    productService.Save();

                    Quantity quantity = new Quantity();
                    quantity.ProductId = product.ProductId;
                    quantity.DateCreated = product.DateCreated;
                    quantity.Quantity1 = productVM.Quantity;
                    quantity.Rate = productVM.Rate;
                    quantityService.Add(quantity);
                    quantityService.Save();

                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Product with Same Category and Supplier exits";
            }

            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName", productVM.CategoryId);
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName", productVM.SupplierId);
            return View(productVM);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productService.LoadByID(id);

            ProductVM productVM = new ProductVM();
            productVM.ProductId = product.ProductId;
            productVM.Descriptions = product.Descriptions;
            productVM.DateCreated = product.DateCreated;
            productVM.CategoryId = product.CategoryId;
            productVM.SupplierId = product.SupplierId;
            productVM.ProductName = product.ProductName;
            productVM.ProductName = product.ProductName;
            productVM.QuantityId = product.Quantities.FirstOrDefault().QuantityId;
            productVM.Quantity = product.Quantities.FirstOrDefault().Quantity1;
            productVM.Rate = product.Quantities.FirstOrDefault().Rate;


            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName", product.SupplierId);
            return View(productVM);
        }

        // POST: ProductVM/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Descriptions,CategoryId,SupplierId,DateCreated,Quantity,Rate,QuantityId")] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                if (productVMService.UpdateValidate(productVM.ProductName,productVM.CategoryId,productVM.SupplierId,productVM.ProductId))
                {
                    Product product = new Product();
                    product.ProductId = productVM.ProductId;
                    product.ProductName = productVM.ProductName;
                    product.CategoryId = productVM.CategoryId;
                    product.SupplierId = productVM.SupplierId;
                    product.Descriptions = productVM.Descriptions;
                    product.DateCreated = productVM.DateCreated;
                    product.DateModfied = DateTime.Now;
                    productService.Update(product);
                    productService.Save();

                    Quantity quantity = new Quantity();
                    quantity.QuantityId = productVM.QuantityId;
                    quantity.ProductId = product.ProductId;
                    quantity.DateCreated = productVM.DateCreated;
                    quantity.DateModfied = product.DateModfied;
                    quantity.Quantity1 = productVM.Quantity;
                    quantity.Rate = productVM.Rate;
                    quantityService.Update(quantity);

                    quantityService.Save();
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Product with Same Category and Supplier exits";
            }
            ViewBag.CategoryId = new SelectList(categoryService.Load(x => x.DateDeleted == null).ToList(), "CategoryId", "CategoryName", productVM.CategoryId);
            ViewBag.SupplierId = new SelectList(supplierService.Load(x => x.DateDeleted == null).ToList(), "SupplierId", "SupplierName", productVM.SupplierId);
            return View(productVM);
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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = productService.LoadByID(id);
            Quantity quantity = product.Quantities.FirstOrDefault();
            if (productVMService.ValidateDelete(product))
            {
                product.DateDeleted = DateTime.Now;
                quantity.DateDeleted = product.DateDeleted;

                productService.Save();
                quantityService.Save();
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
