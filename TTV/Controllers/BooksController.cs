using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTV.Models;

namespace TTV.Controllers
{
    public class BooksController : Controller
    {
        private TangThuVienEntities db = new TangThuVienEntities();

        public ActionResult TruyenDaDang(int? user_id)
        {
            //var user = (TTV.Models.User)HttpContext.Session["user"];
            var books = db.Books.Where(b=>b.user_id == user_id);
            return View(books.ToList());
        }

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.User);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {

            #region follow

            try
            {
                var user = (TTV.Models.User)HttpContext.Session["user"];
                int userId = user.user_id; // Lấy UserId của người dùng hiện tại
                ViewBag.storyId = id;
                ViewBag.countFollow = db.Follows.Count(f=>f.book_id == id);

                var isFollowing = db.Follows.FirstOrDefault(f => f.book_id == id && f.user_id == userId);
                if (isFollowing == null)
                {
                    ViewBag.isFollowing = false;
                }
                else
                {
                    ViewBag.isFollowing = true;
                }
            }
            catch
            {
                ViewBag.isFollowing = false;
                ViewBag.countFollow = db.Follows.Count(f=>f.book_id == id);
            }
            

            #endregion

            #region listChapters
            ViewBag.listChapters = db.Chapters.Where(c => c.book_id == id);
            #endregion

            #region BookCategoty
            var BookCategoty = db.BookCategories.Include(b=>b.Category).Where(b=>b.book_id == id);
            ViewBag.BookCategoty = BookCategoty.ToList();
            #endregion

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "book_id,user_id,book_title,book_author,book_description,book_poster,book_created_at")] Book book, HttpPostedFileBase bookPosterFile)
        {
            if (ModelState.IsValid)
            {

                // Lưu hình ảnh vào thư mục "Images" trong ứng dụng
                if (bookPosterFile != null && bookPosterFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(bookPosterFile.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/asset/img/poster_book/"), fileName);
                    bookPosterFile.SaveAs(imagePath);
                    book.book_poster = fileName;
                }

                // Lưu thông tin sách vào cơ sở dữ liệu
                var user = (TTV.Models.User)HttpContext.Session["user"];
                book.user_id = user.user_id;
                book.book_created_at = DateTime.Now;
                db.Books.Add(book);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return Redirect("~/Books/TruyenDaDang?user_id="+user.user_id);
            }

            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", book.user_id);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", book.user_id);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "book_id,user_id,book_title,book_author,book_description,book_poster,book_created_at")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", book.user_id);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();

            var relativeImagePath = "~/Content/asset/img/poster_book/" + book.book_poster;
            var serverPath = Server.MapPath(relativeImagePath);
            System.IO.File.Delete(serverPath);

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
