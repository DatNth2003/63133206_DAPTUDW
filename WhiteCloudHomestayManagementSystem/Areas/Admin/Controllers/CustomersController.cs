using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.Areas.Admin.Controllers
{
    public class CustomersController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Admin/Customers
        public ActionResult Index(string searchString, int? page, int? pageSize)
        {
            int defaultPageSize = 10;
            int pageNumber = (page ?? 1);
            int itemsPerPage = pageSize ?? defaultPageSize;

            var customers = db.Customers.Include(c => c.User);

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.FullName.Contains(searchString) || c.IdCardNum.Contains(searchString) || c.Phone.Contains(searchString) || c.Email.Contains(searchString));
            }


            ViewBag.CurrentFilter = searchString;

            return View(customers.OrderBy(c => c.FullName).ToPagedList(pageNumber, itemsPerPage));
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Admin/Customers/Create
        public ActionResult Create()
        {
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Admin/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,UserId,FullName,Email,Phone,IdCardImg,IdCardNum,HomestayId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CustomerId = Guid.NewGuid();
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", customer.HomestayId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", customer.UserId);
            return View(customer);
        }

        // GET: Admin/Customers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", customer.HomestayId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", customer.UserId);
            return View(customer);
        }

        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,UserId,FullName,Email,Phone,IdCardImg,IdCardNum,HomestayId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", customer.HomestayId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", customer.UserId);
            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
