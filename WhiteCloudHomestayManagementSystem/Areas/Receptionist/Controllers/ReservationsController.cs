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
    public class ReservationsController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Receptionist/Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Customer).Include(r => r.Employee).Include(r => r.Homestay).Include(r => r.ReservationStatus);
            return View(reservations.ToList());
        }

        // GET: Receptionist/Reservations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Receptionist/Reservations/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FullName");
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name");
            ViewBag.StatusId = new SelectList(db.Reservations, "StatusId", "StatusName");
            return View();
        }

        // POST: Receptionist/Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,CustomerId,HomestayId,EmployeeId,CheckInDate,CheckOutDate,StatusId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.ReservationId = Guid.NewGuid();
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName", reservation.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FullName", reservation.EmployeeId);
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", reservation.HomestayId);
            ViewBag.StatusId = new SelectList(db.Reservations, "StatusId", "StatusName", reservation.StatusId);
            return View(reservation);
        }

        // GET: Receptionist/Reservations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName", reservation.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FullName", reservation.EmployeeId);
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", reservation.HomestayId);
            ViewBag.StatusId = new SelectList(db.ReservationStatuses, "StatusId", "StatusName", reservation.StatusId);
            return View(reservation);
        }

        // POST: Receptionist/Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,CustomerId,HomestayId,EmployeeId,CheckInDate,CheckOutDate,StatusId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName", reservation.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FullName", reservation.EmployeeId);
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", reservation.HomestayId);
            ViewBag.StatusId = new SelectList(db.ReservationStatuses, "StatusId", "StatusName", reservation.StatusId);
            return View(reservation);
        }

        // GET: Receptionist/Reservations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Receptionist/Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
