using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteCloudHomestayManagementSystem.Models;
using WhiteCloudHomestayManagementSystem.ViewModels;

namespace WhiteCloudHomestayManagementSystem.Areas.Receptionist.Controllers
{
    [RouteArea("Receptionist")]
    [RoutePrefix("Booking")]
    [Route("{action=index}")]
    public class ReceptionistBookingController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();
        // GET: Receptionist/ReceptionistBooking
        public ActionResult Index(int? page, string searchString, string sortOrder, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int pageSizeValue = pageSize ?? 10;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CheckInDateSortParm = String.IsNullOrEmpty(sortOrder) ? "checkin_desc" : "";
            ViewBag.CheckOutDateSortParm = sortOrder == "CheckOutDate" ? "checkout_desc" : "CheckOutDate";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.PaidSortParm = sortOrder == "Paid" ? "paid_desc" : "Paid";
            ViewBag.FullNameSortParm = sortOrder == "FullName" ? "fullname_desc" : "FullName";
            ViewBag.HomestayNameSortParm = sortOrder == "HomestayName" ? "homestayname_desc" : "HomestayName";
            ViewBag.CurrentFilter = searchString;
            ViewBag.PageSize = pageSizeValue;

            var bookings = from b in db.Bookings select b;

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

            if (bookings.Any())
            {
                return View(bookings.ToPagedList(pageNumber, pageSizeValue));
            }
            else
            {
                return HttpNotFound("No bookings found.");
            }
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
        public ActionResult CheckIn()
        {
            var homestays = db.Homestays.Where(h => h.Status == "Available").ToList();
            var viewModel = new CheckInVM
            {
                Homestays = homestays,
                Booking = new Booking(),
                Customer = new Customer()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CheckIn(CheckInVM viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(viewModel.Customer);
                db.SaveChanges();

                viewModel.Booking.CustomerID = viewModel.Customer.CustomerID;
                viewModel.Booking.Status = "CheckedIn";
                db.Bookings.Add(viewModel.Booking);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            viewModel.Homestays = db.Homestays.Where(h => h.Status == "Available").ToList();
            return View(viewModel);
        }




        [HttpPost]
        public ActionResult CheckoutConfirmed(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            booking.Status = "CheckedOut";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult EditBooking(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public ActionResult EditBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        [HttpPost]
        public ActionResult DeleteBooking(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            // Xóa booking khỏi cơ sở dữ liệu
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}