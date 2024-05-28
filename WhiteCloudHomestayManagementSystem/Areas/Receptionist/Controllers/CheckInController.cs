using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteCloudHomestayManagementSystem.Models;
using WhiteCloudHomestayManagementSystem.ViewModels;

namespace WhiteCloudHomestayManagementSystem.Areas.Receptionist.Controllers
{
    public class CheckInController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Receptionist/CheckIn
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers.ToList(), "CustomerId", "FullName");
            ViewBag.HomestayId = new SelectList(db.Homestays.ToList(), "HomestayId", "Name");
            ViewBag.EmployeeId = new SelectList(db.Employees.ToList(), "EmployeeId", "FullName");
            ViewBag.PaymentTypeId = new SelectList(db.PaymentTypes.ToList(), "PaymentTypeId", "TypeName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CheckInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation
                {
                    ReservationId = Guid.NewGuid(),
                    CustomerId = viewModel.CustomerId,
                    HomestayId = viewModel.HomestayId,
                    EmployeeId = viewModel.EmployeeId,
                    CheckInDate = viewModel.CheckInDate,
                    CheckOutDate = viewModel.CheckOutDate,
                    NumOfAdults = viewModel.NumberOfAdults,
                    NumOfChild = viewModel.NumberOfChildren,
                    SpecialRequests = viewModel.SpecialRequests,
                    Notes = viewModel.Notes
                };

                db.Reservations.Add(reservation);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Customers = new SelectList(db.Customers.ToList(), "CustomerId", "FullName");
            ViewBag.Homestays = new SelectList(db.Homestays.ToList(), "HomestayId", "Name");
            ViewBag.Employees = new SelectList(db.Employees.ToList(), "EmployeeId", "FullName");
            ViewBag.PaymentTypes = new SelectList(db.PaymentTypes.ToList(), "PaymentTypeId", "TypeName");
            return View(viewModel);
        }



    }
}