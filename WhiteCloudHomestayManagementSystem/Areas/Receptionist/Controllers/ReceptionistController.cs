using System.Web.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


    }
}
