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
using WhiteCloudHomestayManagementSystem.ViewModels;

namespace WhiteCloudHomestayManagementSystem.Areas.Receptionist.Controllers
{
    public class ReservationsController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Receptionist/Reservations
        public ActionResult Index(string searchString, int? page)
        {
            var reservations = db.Reservations.Include(r => r.Customer)
                                               .Include(r => r.Homestay)
                                               .Include(r => r.ReservationStatus)
                                               .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                reservations = reservations.Where(r => r.Customer.FullName.Contains(searchString)
                                                    || r.Homestay.Name.Contains(searchString));
            }

            reservations = reservations.OrderBy(r => r.ReservationId);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reservations.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Guid? homestayId)
        {

            if (homestayId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var homestay = db.Homestays.Find(homestayId);
            if (homestay == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FullName");
            ViewBag.Homestay = homestay;

            var reservation = new Reservation
            {
                HomestayId = homestay.HomestayId,
                StatusId = 1
            };

            return View(reservation);
        }
        // POST: Receptionist/Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation, NewCustomerVM newCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();

                if (!string.IsNullOrEmpty(newCustomer.FullName) && !string.IsNullOrEmpty(newCustomer.Email) && !string.IsNullOrEmpty(newCustomer.Phone))
                {
                    var customer = new Customer
                    {
                        FullName = newCustomer.FullName,
                        Email = newCustomer.Email,
                        Phone = newCustomer.Phone
                    };

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    reservation.CustomerId = customer.CustomerId;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Homestay = db.Homestays.Find(reservation.HomestayId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName", reservation.CustomerId);
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
