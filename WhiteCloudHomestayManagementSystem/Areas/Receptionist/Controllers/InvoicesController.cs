using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.Areas.Receptionist.Controllers
{
    public class InvoicesController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Receptionist/Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.Reservation).Include(i => i.InvoiceStatus);
            return View(invoices.ToList());
        }

        // GET: Receptionist/Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Receptionist/Invoices/Create
        public ActionResult Create()
        {
            ViewBag.ReservationId = new SelectList(db.Reservations, "ReservationId", "SpecialRequests");
            ViewBag.StatusId = new SelectList(db.InvoiceStatuses, "Id", "StatusName");
            return View();
        }

        // POST: Receptionist/Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,ReservationId,StatusId,IssuedDate,CheckOutDate,HomestayCharge,ServiceCharge,Surcharge,TotalAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReservationId = new SelectList(db.Reservations, "ReservationId", "SpecialRequests", invoice.ReservationId);
            ViewBag.StatusId = new SelectList(db.InvoiceStatuses, "Id", "StatusName", invoice.StatusId);
            return View(invoice);
        }

        // GET: Receptionist/Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReservationId = new SelectList(db.Reservations, "ReservationId", "SpecialRequests", invoice.ReservationId);
            ViewBag.StatusId = new SelectList(db.InvoiceStatuses, "Id", "StatusName", invoice.StatusId);
            return View(invoice);
        }

        // POST: Receptionist/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,ReservationId,StatusId,IssuedDate,CheckOutDate,HomestayCharge,ServiceCharge,Surcharge,TotalAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReservationId = new SelectList(db.Reservations, "ReservationId", "SpecialRequests", invoice.ReservationId);
            ViewBag.StatusId = new SelectList(db.InvoiceStatuses, "Id", "StatusName", invoice.StatusId);
            return View(invoice);
        }

        // GET: Receptionist/Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Receptionist/Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
