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

namespace WhiteCloudHomestayManagementSystem.Areas.Manager.Controllers
{
    public class ReservationsController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Manager/Reservations
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CheckInDateSortParm = String.IsNullOrEmpty(sortOrder) ? "checkin_date_desc" : "";
            ViewBag.CheckOutDateSortParm = sortOrder == "CheckOutDate" ? "checkout_date_desc" : "CheckOutDate";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var reservations = db.Reservations.Include(r => r.Customer)
                                              .Include(r => r.Employee)
                                              .Include(r => r.Homestay)
                                              .Include(r => r.ReservationStatus);

            if (!String.IsNullOrEmpty(searchString))
            {
                reservations = reservations.Where(r => r.Customer.FullName.Contains(searchString)
                                               || r.Homestay.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "checkin_date_desc":
                    reservations = reservations.OrderByDescending(r => r.CheckInDate);
                    break;
                case "CheckOutDate":
                    reservations = reservations.OrderBy(r => r.CheckOutDate);
                    break;
                case "checkout_date_desc":
                    reservations = reservations.OrderByDescending(r => r.CheckOutDate);
                    break;
                default:
                    reservations = reservations.OrderBy(r => r.CheckInDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reservations.ToPagedList(pageNumber, pageSize));
        }


        // GET: Manager/Reservations/Details/5
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

        // GET: Manager/Reservations/Create
        public ActionResult Create(string customerSearch, string employeeSearch, string homestaySearch)
        {
            var customers = string.IsNullOrEmpty(customerSearch)
                ? db.Customers.ToList()
                : db.Customers.Where(c => c.UserId.Contains(customerSearch)).ToList();

            var employees = string.IsNullOrEmpty(employeeSearch)
                ? db.Employees.ToList()
                : db.Employees.Where(e => e.UserId.Contains(employeeSearch)).ToList();

            var homestays = string.IsNullOrEmpty(homestaySearch)
                ? db.Homestays.ToList()
                : db.Homestays.Where(h => h.Name.Contains(homestaySearch)).ToList();

            var reservationStatuses = db.ReservationStatuses.ToList();

            ViewBag.Customers = new SelectList(customers, "CustomerId", "UserId");
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "UserId");
            ViewBag.Homestays = new SelectList(homestays, "HomestayId", "Name");
            ViewBag.Statuses = new SelectList(reservationStatuses, "Id", "StatusName");

            ViewBag.CustomerSearch = customerSearch;
            ViewBag.EmployeeSearch = employeeSearch;
            ViewBag.HomestaySearch = homestaySearch;

            return View();
        }



        // POST: Manager/Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,CustomerId,HomestayId,EmployeeId,CheckInDate,CheckOutDate,StatusId,NumberOfAdults,NumberOfChildren,SpecialRequests,Notes")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.ReservationId = Guid.NewGuid();
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserId", reservation.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "UserId", reservation.EmployeeId);
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", reservation.HomestayId);
            ViewBag.StatusId = new SelectList(db.ReservationStatuses, "Id", "StatusName", reservation.StatusId);
            return View(reservation);
        }

        // GET: Manager/Reservations/Edit/5
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
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserId", reservation.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "UserId", reservation.EmployeeId);
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", reservation.HomestayId);
            ViewBag.StatusId = new SelectList(db.ReservationStatuses, "Id", "StatusName", reservation.StatusId);
            return View(reservation);
        }

        // POST: Manager/Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,CustomerId,HomestayId,EmployeeId,CheckInDate,CheckOutDate,StatusId,NumberOfAdults,NumberOfChildren,SpecialRequests,Notes")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserId", reservation.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "UserId", reservation.EmployeeId);
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", reservation.HomestayId);
            ViewBag.StatusId = new SelectList(db.ReservationStatuses, "Id", "StatusName", reservation.StatusId);
            return View(reservation);
        }

        // GET: Manager/Reservations/Delete/5
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

        // POST: Manager/Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatus(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            reservation.StatusId = id;
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
