using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.Areas.Receptionist.Controllers
{
    public class ReceptionistController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Receptionist/Receptionist
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateBooking()
        {
            ViewBag.Homestays = db.Homestays.ToList();
            ViewBag.Customers = db.Customers.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Homestays = db.Homestays.ToList();
            ViewBag.Customers = db.Customers.ToList();
            return View(booking);
        }

        public ActionResult BookingList(int? page, string searchString, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CheckInDateSortParm = String.IsNullOrEmpty(sortOrder) ? "checkin_desc" : "";
            ViewBag.CheckOutDateSortParm = sortOrder == "CheckOutDate" ? "checkout_desc" : "CheckOutDate";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.PaidSortParm = sortOrder == "Paid" ? "paid_desc" : "Paid";
            ViewBag.FullNameSortParm = sortOrder == "FullName" ? "fullname_desc" : "FullName";
            ViewBag.HomestayNameSortParm = sortOrder == "HomestayName" ? "homestayname_desc" : "HomestayName";

            ViewBag.CurrentFilter = searchString;

            var bookings = from b in db.Bookings
                           select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b => b.Customer.FullName.Contains(searchString)
                                            || b.Homestay.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "checkin_desc":
                    bookings = bookings.OrderByDescending(b => b.CheckInDate);
                    break;
                case "CheckOutDate":
                    bookings = bookings.OrderBy(b => b.CheckOutDate);
                    break;
                case "checkout_desc":
                    bookings = bookings.OrderByDescending(b => b.CheckOutDate);
                    break;
                case "Status":
                    bookings = bookings.OrderBy(b => b.Status);
                    break;
                case "status_desc":
                    bookings = bookings.OrderByDescending(b => b.Status);
                    break;
                case "Paid":
                    bookings = bookings.OrderBy(b => b.Paid);
                    break;
                case "paid_desc":
                    bookings = bookings.OrderByDescending(b => b.Paid);
                    break;
                case "FullName":
                    bookings = bookings.OrderBy(b => b.Customer.FullName);
                    break;
                case "fullname_desc":
                    bookings = bookings.OrderByDescending(b => b.Customer.FullName);
                    break;
                case "HomestayName":
                    bookings = bookings.OrderBy(b => b.Homestay.Name);
                    break;
                case "homestayname_desc":
                    bookings = bookings.OrderByDescending(b => b.Homestay.Name);
                    break;
                default:
                    bookings = bookings.OrderBy(b => b.CheckInDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(bookings.ToPagedList(pageNumber, pageSize));
        }



        public ActionResult BookingDetails(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public ActionResult CheckIn(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                booking.Status = "CheckedIn";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult CheckOut(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                booking.Status = "CheckedOut";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

    }
}