using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Complete.Domain;
using Complete.Repository.Repositories;
using Complete.Service.EntityServices;
using Complete.Web.ViewModel;

namespace Complete.Web.Controllers
{
    public class BillingsController : Controller
    {
        private ProductService productService = new ProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Product>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private BillingService billingService = new BillingService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Billing>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private BillingProductService billingProductService = new BillingProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<BillingProduct>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));

        public ActionResult Index()
        {
            var billing = billingService.Load(x => x.DateDeleted == null).ToList();
            return View(billing.ToList());
        }

        public ActionResult BillingDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var billingdetails = billingProductService.Load(x => x.BillingId == id).ToList();
            if (billingdetails == null)
            {
                return HttpNotFound();
            }
            return View(billingdetails);
        }

        public ActionResult Billing()
        {
            var datas = productService.Load(x => x.DateDeleted == null).ToList();
                List<BillingProductVM> billingProductVMs = new List<BillingProductVM>();
                foreach (var item in datas)
                {
                    BillingProductVM billingProductVM = new BillingProductVM();
                    billingProductVM.ProductId = item.ProductId;
                    billingProductVM.ProductName = item.ProductName;
                    billingProductVM.Descriptions = item.Descriptions;
                    billingProductVM.SupplierName = item.Supplier.SupplierName;
                    billingProductVM.CategoryName = item.Category.CategoryName;
                    billingProductVM.Quantity = item.Quantities.First().Quantity1;
                    billingProductVM.Rate = item.Quantities.First().Rate;
                    billingProductVMs.Add(billingProductVM);
                }

                return View(billingProductVMs);
            
        }





        [HttpPost]
        public JsonResult Create(List<BillingProductVM> billingProductViewModel, String customername, String address, String contact, String gt, String total, String discount)
        {
            if (billingProductViewModel != null)
            {
                Billing billing = new Billing();
                billing.DateCreated = DateTime.Now;
                billing.CreatedBy = 1;
                billing.Amount = gt;
                billing.TotalDiscount = discount;
                billing.TotalPrice = total;
                billing.Name = customername;
                billing.Addres = address;
                billing.ContactNo = contact;
                billingService.Add(billing);
                billingService.Save();

               int BillingId = billing.BillingId;
                foreach (var item in billingProductViewModel)
                {
                    BillingProduct billingProduct = new BillingProduct();
                    billingProduct.Discount = item.Discount;
                    billingProduct.BillingId = BillingId;
                    billingProduct.Quantity = item.Quantity;
                    billingProduct.ProductId = item.ProductId;

                    Product product = productService.LoadByID(billingProduct.ProductId);
                    product.Quantities.FirstOrDefault().Quantity1 -= billingProduct.Quantity;

                    billingProductService.Add(billingProduct);
                }

                productService.Save();
                billingProductService.Save();
            }

            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = new { d = "ddd" };
            return jsonResult;
        }









        //// GET: Billings/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "UserName");
        //    return View();
        //}

        //// POST: Billings/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "BillingId,TotalPrice,TotalDiscount,Amount,CreatedBy,Name,Addres,ContactNo,DateCreated,DateModfied,DateDeleted")] Billing billing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Billings.Add(billing);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "UserName", billing.CreatedBy);
        //    return View(billing);
        //}

        //// GET: Billings/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Billing billing = db.Billings.Find(id);
        //    if (billing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "UserName", billing.CreatedBy);
        //    return View(billing);
        //}

        //// POST: Billings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "BillingId,TotalPrice,TotalDiscount,Amount,CreatedBy,Name,Addres,ContactNo,DateCreated,DateModfied,DateDeleted")] Billing billing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(billing).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "UserName", billing.CreatedBy);
        //    return View(billing);
        //}

        //// GET: Billings/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Billing billing = db.Billings.Find(id);
        //    if (billing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(billing);
        //}

        //// POST: Billings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Billing billing = db.Billings.Find(id);
        //    db.Billings.Remove(billing);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                billingService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
