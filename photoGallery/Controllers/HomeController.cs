using photoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace photoGallery.Controllers
{
    public class HomeController : Controller
    {
        private GalleryContext db = new GalleryContext();
        public ActionResult Index()
        {
            return View(db.ImageGalleries.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}