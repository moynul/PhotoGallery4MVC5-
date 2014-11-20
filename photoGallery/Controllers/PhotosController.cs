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
    [HandleError]
    public class PhotosController : Controller
    {
        private GalleryContext db = new GalleryContext();

        // GET: Photos
         [HandleError]
        public ActionResult Index()
        {                      
            var photos = db.Photos.Include(p => p.Album);
            return View(photos.ToList());
        }
        
        public ActionResult filterPhoto(int albamId) 
        {
            var photos = db.Photos.Include(p => p.Album);
            photos = photos.Where(c => c.AlbumId == albamId);
            return View("PhotoIndex",photos.ToList());
        }
        public ActionResult Manage()
        {
            var photos = db.Photos.Include(p => p.Album);
            return View("ManagePhoto",photos.ToList());
        }
        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.ImageGalleries, "ID", "AlbumName");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,AlbumId")] Photo photo, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Message"] = "";
                    var filename = "";
                    if (file != null && file.ContentLength > 0)
                    {
                        Guid gid;
                        gid = Guid.NewGuid();
                        var extension = Path.GetExtension(file.FileName);
                        filename = gid + extension;

                        var path = Path.Combine(Server.MapPath("~/Content/PhotoGallery/" + setAlbumName(photo.AlbumId)), filename);

                        var data = new byte[file.ContentLength];
                        file.InputStream.Read(data, 0, file.ContentLength);

                        using (var sw = new FileStream(path, FileMode.Create))
                        {
                            sw.Write(data, 0, data.Length);
                        }
                    }
                    photo.ImageName = filename;
                    photo.InsertedDateTime = DateTime.Now;
                    photo.InsertedBy = User.Identity.Name;
                    db.Photos.Add(photo);
                    db.SaveChanges();
                    TempData["Message"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Success!</strong> Successfully Save.</div> ";
                }
                catch (Exception ex)
                {

                    TempData["Message"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times; Directory</a><strong> " + ex.ToString() + "</strong> Not found.</div>";
                }
                return RedirectToAction("Index");
            }

            ViewBag.AlbumId = new SelectList(db.ImageGalleries, "ID", "AlbumName", photo.AlbumId);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.ImageGalleries, "ID", "AlbumName", photo.AlbumId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,AlbumId")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Message"] = "";

                    db.Entry(photo).State = EntityState.Modified;
                    db.Entry(photo).Property("InsertedBy").IsModified = false;
                    db.Entry(photo).Property("InsertedDateTime").IsModified = false;
                    db.Entry(photo).Property("ImageName").IsModified = false;
                    string path = Server.MapPath("~/Content/PhotoGallery/");
                    var ImageName = GetImageName(photo.ID);
                    string Fromfol = GetAlbumNameByPhotoID(photo.ID) + "/" + ImageName;
                    string Tofol = setAlbumName(photo.AlbumId) + "/" + ImageName;
                    photo.LastUpdatedBy = User.Identity.Name;
                    photo.LastUpdatedDatetime = DateTime.Now;
                    System.IO.File.Move(path + Fromfol, path + Tofol);                    
                    db.SaveChanges();
                    TempData["Message"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Success!</strong> Successfully Edited.</div> ";
                }
                catch (IOException ex)
                {

                    TempData["Message"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times; </a><strong> " + ex.ToString() + "</strong> </div>";
                }
                return RedirectToAction("Index");
            }
            ViewBag.AlbumId = new SelectList(db.ImageGalleries, "ID", "AlbumName", photo.AlbumId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Photo photo = db.Photos.Find(id);
                var path = Server.MapPath("~/Content/PhotoGallery/" + photo.Album.AlbumName);
                var filename = Path.Combine(Server.MapPath("~/Content/PhotoGallery/" + photo.Album.AlbumName), photo.ImageName);
                if (directoryExists(path))
                {

                    System.IO.File.Delete(filename);
                    db.Photos.Remove(photo);
                    db.SaveChanges();
                    TempData["Message"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Success!</strong> Successfully deleted.</div> ";
                }
                else
                {
                    TempData["Message"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times; Directory</a><strong> " + photo.Album.AlbumName + "</strong> Not found.</div>";
                }
            }
            catch (IOException ex)
            {
                TempData["Message"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times; </a><strong> " + ex.ToString() + "</strong> </div>";
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


        private string setAlbumName(int ID) {

            Album abl = null;
            using (GalleryContext Newdb = new GalleryContext())
            {
                abl = (from s in Newdb.ImageGalleries
                       where s.ID == ID
                       select s).FirstOrDefault();

            }
            return abl.AlbumName;
        }

        private string GetAlbumNameByPhotoID(int ID)
        {

            var albumName = "";
            using (GalleryContext Newdb = new GalleryContext())
            {
                var photos = Newdb.Photos.Include(p => p.Album);
                photos = photos.Where(c => c.ID == ID);
                if (photos.Count() > 0)
                {
                    albumName = photos.FirstOrDefault().Album.AlbumName;
                }
            }
            return albumName;
        }

        private string GetImageName(int ID)
        {

            var ImageName = "";
            using (GalleryContext Newdb = new GalleryContext())
            {
                var Photos = (from s in Newdb.Photos
                              where s.ID == ID
                              select s);
                if (Photos.Count() > 0)
                {
                    ImageName = Photos.FirstOrDefault().ImageName;
                }
            }
            return ImageName;
        }

        private bool directoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
