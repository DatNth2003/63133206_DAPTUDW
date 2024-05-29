﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.Areas.Manager.Controllers
{
    public class CustomersController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Manager/Customers
        public ActionResult Index(string searchString, int? page)
        {
            var customers = db.Customers.Include(c => c.User);

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.FullName.Contains(searchString) || c.IdCardNum.Contains(searchString) || c.Phone.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.CurrentFilter = searchString;

            return View(customers.OrderBy(c => c.FullName).ToPagedList(pageNumber, pageSize));
        }


        // GET: Manager/Customers/Details/5
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

        // GET: Manager/Customers/Create
        public ActionResult Create(string homestaySearch)
        {
            var homestays = string.IsNullOrEmpty(homestaySearch)
                ? db.Homestays.ToList()
                : db.Homestays.Where(h => h.Name.Contains(homestaySearch)).ToList();
            ViewBag.Homestays = new SelectList(homestays, "HomestayId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Manager/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,UserId,FullName,Email,Phone,IdCardNum,HomestayId")] Customer customer, HttpPostedFileBase IdCardImg)
        {
            if (ModelState.IsValid)
            {
                if (IdCardImg != null && IdCardImg.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(IdCardImg.FileName);
                    string directoryPath = Server.MapPath("~/Uploads/Images/Customers/IdCardImgs");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, fileName);
                    IdCardImg.SaveAs(filePath);
                    customer.IdCardImg = "/Uploads/Images/Customers/IdCardImgs/" + fileName;
                }
                customer.CustomerId = Guid.NewGuid();
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", customer.HomestayId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", customer.UserId);
            return View(customer);
        }


        // GET: Manager/Customers/Edit/5
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

        // POST: Manager/Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,UserId,FullName,Email,Phone,IdCardNum,HomestayId")] Customer customer, HttpPostedFileBase IdCardImg)
        {
            if (ModelState.IsValid)
            {
                if (IdCardImg != null && IdCardImg.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(IdCardImg.FileName);
                    string directoryPath = @"D:\GitHub\63133206_DAPTUDW\WhiteCloudHomestayManagementSystem\App_Data\Images\Customers\IdCardImgs";
                    string path = Path.Combine(directoryPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    IdCardImg.SaveAs(path);
                    customer.IdCardImg = Path.Combine("/App_Data/Images/Customers/IdCardImgs/", fileName);  // Relative path for storing in the database
                }
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomestayId = new SelectList(db.Homestays, "HomestayId", "Name", customer.HomestayId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", customer.UserId);
            return View(customer);
        }


        // GET: Manager/Customers/Delete/5
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

        // POST: Manager/Customers/Delete/5
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