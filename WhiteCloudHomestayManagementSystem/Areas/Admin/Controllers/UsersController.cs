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

namespace WhiteCloudHomestayManagementSystem.Areas.Admin.Controllers
{

    public class UsersController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Admin/Users
        public ActionResult Index(string searchString, string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RoleSortParm = sortOrder == "Role" ? "role_desc" : "Role";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            var users = from u in db.Users select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                case "Role":
                    users = users.OrderBy(u => u.Roles.FirstOrDefault().Id);
                    break;
                case "role_desc":
                    users = users.OrderByDescending(u => u.Roles.FirstOrDefault().Id);
                    break;
                case "Email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                default:
                    users = users.OrderBy(u => u.UserName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            var roles = db.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = roles;
            return View(new RegisterViewModel());
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Email = model.Email };

                db.Users.Add(user);
                db.SaveChanges();

                if (selectedRoles != null)
                {
                    foreach (var roleName in selectedRoles)
                    {
                        var role = db.Roles.FirstOrDefault(r => r.Name == roleName);
                        if (role != null)
                        {
                            user.Roles.Add(role);
                        }
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            var roles = db.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = roles;
            return View(model);
        }



        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = user.Roles.Select(r => r.Id).ToList();

            ViewBag.Roles = new MultiSelectList(
                db.Roles.ToList(),
                "Id",
                "Name",
                userRoles
            );

            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,UserName,FullName")] User user, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var userInDb = db.Users.Include(u => u.Roles).SingleOrDefault(u => u.Id == user.Id);
                if (userInDb == null)
                {
                    return HttpNotFound();
                }

                userInDb.Email = user.Email;
                userInDb.EmailConfirmed = user.EmailConfirmed;
                userInDb.PasswordHash = user.PasswordHash;
                userInDb.SecurityStamp = user.SecurityStamp;
                userInDb.PhoneNumber = user.PhoneNumber;
                userInDb.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                userInDb.TwoFactorEnabled = user.TwoFactorEnabled;
                userInDb.LockoutEnd = user.LockoutEnd;
                userInDb.LockoutEnabled = user.LockoutEnabled;
                userInDb.AccessFailedCount = user.AccessFailedCount;
                userInDb.UserName = user.UserName;
                userInDb.FullName = user.FullName;

                userInDb.Roles.Clear();
                if (selectedRoles != null)
                {
                    foreach (var roleId in selectedRoles)
                    {
                        var role = db.Roles.Find(roleId);
                        userInDb.Roles.Add(role);
                    }
                }

                db.Entry(userInDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Id", "Name", selectedRoles);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
