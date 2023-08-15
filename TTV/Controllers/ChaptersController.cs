using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTV.Models;

namespace TTV.Controllers
{
    public class ChaptersController : Controller
    {
        private TangThuVienEntities db = new TangThuVienEntities();

        // GET: Chapters
        public ActionResult Index()
        {
            var chapters = db.Chapters.Include(c => c.Book);
            return View(chapters.ToList());
        }

        // GET: Chapters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }

            Book book = db.Books.Find(chapter.book_id); ViewBag.book = book;

            #region Data for comment
            ViewBag.user = (TTV.Models.User)HttpContext.Session["user"];
            ViewBag.chapterCommentList = db.Comments.Include(c => c.User).Where(c=>c.chapter_id == id).ToList();
            ViewBag.bookCommentList = db.Comments.Include(c=>c.Chapter).Where(c=>c.Chapter.book_id == book.book_id).ToList();
            ViewBag.ChapterID = id;

            #endregion

            return View(chapter);
        }

        // GET: Chapters/Create
        public ActionResult Create()
        {
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title");
            return View();
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "chapter_id,book_id,chapter_number,chapter_title,chapter_content,chapter_view,chapter_created_at")] Chapter chapter, int book_id)
        {
            var user = (TTV.Models.User)HttpContext.Session["user"];
            if (ModelState.IsValid)
            {
                chapter.book_id = book_id;
                chapter.chapter_created_at = DateTime.Now;
                chapter.chapter_view = 0;

                db.Chapters.Add(chapter);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return Redirect("~/Books/TruyenDaDang?user_id="+user.user_id);
            }

            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", chapter.book_id);
            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", chapter.book_id);
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chapter_id,book_id,chapter_number,chapter_title,chapter_content,chapter_view,chapter_created_at")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", chapter.book_id);
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            db.Chapters.Remove(chapter);
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
