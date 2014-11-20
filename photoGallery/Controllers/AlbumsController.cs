using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using photoGallery.Models;
using System.IO;

namespace photoGallery.Controllers
{
    public class AlbumsController : Controller
    {
        private GalleryContext db = new GalleryContext();

        // GET: Albums
        public ActionResult Index()
        {            
            return View(db.ImageGalleries.ToList());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.ImageGalleries.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AlbumName,InsertedDateTime,LastUpdatedDatetime,InsertedBy,LastUpdatedBy")] Album album)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "";                
                var path = Server.MapPath("~/Content/PhotoGallery/" + album.AlbumName);
                Directory.CreateDirectory(path);
                db.ImageGalleries.Add(album);
                album.InsertedDateTime = DateTime.Now;
                album.InsertedBy = User.Identity.Name;
                db.SaveChanges();
                TempData["Message"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Success!</strong> Successfully Save.</div> ";
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.ImageGalleries.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AlbumName,InsertedDateTime,LastUpdatedDatetime,InsertedBy,LastUpdatedBy")] Album album)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "";
                db.Entry(album).State = EntityState.Modified;
                album.LastUpdatedBy = User.Identity.Name;
                album.LastUpdatedDatetime = DateTime.Now;
                db.Entry(album).Property("InsertedBy").IsModified = false;
                db.Entry(album).Property("InsertedDateTime").IsModified = false;
                
                string path = Server.MapPath("~/Content/PhotoGallery/");             
                string Fromfol = getAlbamName(album.ID);
                string Tofol = album.AlbumName;
                if (directoryExists(path + Fromfol))
                {
                    Directory.Move(path + Fromfol, path + Tofol);
                    db.SaveChanges();
                    TempData["Message"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Success!</strong> Successfully renamed.</div> ";
                }
                else {
                    TempData["Message"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times; Directory</a><strong> " + Fromfol + "</strong> Not found.</div>";
                }                                
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.ImageGalleries.Find(id);

            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["Message"] = "";
            Album album = db.ImageGalleries.Find(id);
            db.ImageGalleries.Remove(album);
            var path = Server.MapPath("~/Content/PhotoGallery/" + album.AlbumName);
            if (directoryExists(path))
            {
                Directory.Delete(path, true);
                db.SaveChanges();
                TempData["Message"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Success!</strong> Successfully deleted.</div> ";
            }
            else
            {
                TempData["Message"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times; Directory</a><strong> " + album.AlbumName + "</strong> Not found.</div>";
            }
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
        private string getAlbamName(int Id)
        {
            Album abl = null;           
            using (GalleryContext Newdb = new GalleryContext())
            {
                abl = (from s in Newdb.ImageGalleries
                        where s.ID == Id
                        select s).FirstOrDefault();

            }
            return abl.AlbumName;
        }
        private bool directoryExists( string path)
        {
            return Directory.Exists(path);
        }
    }
}
