using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteCloudHomestayManagementSystem.Models;
using WhiteCloudHomestayManagementSystem.ViewModels;

namespace WhiteCloudHomestayManagementSystem.Areas.Receptionist.Controllers
{
    public class ReceptionistsController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Receptionist/Receptionists
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewCheckIn(Guid homestayId)
        {
            var homestay = db.Homestays.Include("Customers").FirstOrDefault(h => h.HomestayId == homestayId);

            if (homestay == null)
            {
                return HttpNotFound();
            }

            ViewBag.Homestay = homestay;
            ViewBag.CustomerId = new SelectList(homestay.Customers, "CustomerId", "FullName");
            ViewBag.EmployeeId = new SelectList(db.Employees.ToList(), "EmployeeId", "FullName");
            ViewBag.PaymentTypeId = new SelectList(db.PaymentTypes.ToList(), "PaymentTypeId", "TypeName");

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCheckIn(CheckInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer;
                if (viewModel.CustomerId != null)
                {
                    customer = db.Customers.Find(viewModel.CustomerId);
                    if (customer == null)
                    {
                        ModelState.AddModelError("", "Selected customer does not exist.");
                        ViewBag.HomestayId = new SelectList(db.Homestays.ToList(), "HomestayId", "Name", viewModel.HomestayId);
                        ViewBag.EmployeeId = new SelectList(db.Employees.ToList(), "EmployeeId", "FullName", viewModel.EmployeeId);
                        ViewBag.PaymentTypeId = new SelectList(db.PaymentTypes.ToList(), "PaymentTypeId", "TypeName", viewModel.PaymentTypeId);
                        return View(viewModel);
                    }
                }
                else
                {
                    customer = new Customer
                    {
                        CustomerId = Guid.NewGuid(),
                        FullName = viewModel.Customer.FullName,
                        // Gán các trường thông tin khác cho customer tại đây
                    };
                    db.Customers.Add(customer);
                }

                Reservation reservation = new Reservation
                {
                    ReservationId = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
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

                return RedirectToAction("Index", "Reservation");
            }

            ViewBag.HomestayId = new SelectList(db.Homestays.ToList(), "HomestayId", "Name", viewModel.HomestayId);
            ViewBag.EmployeeId = new SelectList(db.Employees.ToList(), "EmployeeId", "FullName", viewModel.EmployeeId);
            ViewBag.PaymentTypeId = new SelectList(db.PaymentTypes.ToList(), "PaymentTypeId", "TypeName", viewModel.PaymentTypeId);
            return View(viewModel);
        }
    }
}