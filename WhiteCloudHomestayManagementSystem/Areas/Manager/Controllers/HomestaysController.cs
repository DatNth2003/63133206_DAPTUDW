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
    public class HomestaysController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Manager/Homestays
        public ActionResult Index(int? page, string searchString, int? pageSize)
        {
            ViewBag.CurrentFilter = searchString;

            var homestays = db.Homestays.Include(h => h.HomestayStatus);

            if (!String.IsNullOrEmpty(searchString))
            {
                homestays = homestays.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
            }

            homestays = homestays.OrderBy(s => s.Name);
            int pageNumber = (page ?? 1);
            int size = (pageSize ?? 5);
            int sizeToUse = size < 1 ? 1 : size;

            ViewBag.HomestayStatuses = new SelectList(db.HomestayStatuses, "Id", "StatusName");

            return View(homestays.ToPagedList(pageNumber, sizeToUse));
        }



        // GET: Manager/Homestays/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Homestay homestay = db.Homestays.Include(h => h.Customers).FirstOrDefault(h => h.HomestayId == id);
            if (homestay == null)
            {
                return HttpNotFound();
            }
            return View(homestay);
        }

        // GET: Manager/Homestays/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.HomestayStatuses, "Id", "StatusName");
            return View();
        }

        // POST: Manager/Homestays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HomestayId,Name,Address,Description,Price,ContactPhone,ContactEmail,Capacity,StatusId")] Homestay homestay)
        {
            if (ModelState.IsValid)
            {
                homestay.HomestayId = Guid.NewGuid();
                db.Homestays.Add(homestay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.HomestayStatuses, "Id", "StatusName", homestay.StatusId);
            return View(homestay);
        }

        // GET: Manager/Homestays/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Homestay homestay = db.Homestays.Find(id);
            if (homestay == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.HomestayStatuses, "Id", "StatusName", homestay.StatusId);
            return View(homestay);
        }

        // POST: Manager/Homestays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HomestayId,Name,Address,Description,Price,ContactPhone,ContactEmail,Capacity,StatusId")] Homestay homestay)
        {
            if (ModelState.IsValid)
            {
                var existingHomestay = db.Homestays.Find(homestay.HomestayId);

                if (existingHomestay.Customers.Any() && existingHomestay.StatusId == 2)
                {
                    homestay.StatusId = 1;
                }
                else if (!existingHomestay.Customers.Any() && existingHomestay.StatusId == 1)
                {
                    homestay.StatusId = 2;
                }


                // Cập nhật trạng thái homestay và lưu vào database
                db.Entry(homestay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // Trường hợp ModelState không hợp lệ, hiển thị lại form với thông tin đã nhập và thông báo lỗi
            ViewBag.StatusId = new SelectList(db.HomestayStatuses, "Id", "StatusName", homestay.StatusId);
            return View(homestay);
        }




        // GET: Manager/Homestays/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Homestay homestay = db.Homestays.Find(id);
            if (homestay == null)
            {
                return HttpNotFound();
            }
            return View(homestay);
        }

        // POST: Manager/Homestays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Homestay homestay = db.Homestays.Find(id);
            db.Homestays.Remove(homestay);
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
