using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.Areas.Staff.Controllers
{
    public class HomestaysController : Controller
    {
        private DBWhiteCloudEntities db = new DBWhiteCloudEntities();

        // GET: Staff/Homestays
        public ActionResult Index(string searchString, int? page)
        {
            var homestays = db.Homestays.Include(h => h.HomestayStatus);

            if (!String.IsNullOrEmpty(searchString))
            {
                homestays = homestays.Where(h => h.Name.Contains(searchString) || h.Address.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(homestays.OrderBy(h => h.Name).ToPagedList(pageNumber, pageSize));
        }
    }
}
