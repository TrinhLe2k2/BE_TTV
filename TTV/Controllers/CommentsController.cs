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
    public class CommentsController : Controller
    {
        private TangThuVienEntities db = new TangThuVienEntities();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Chapter).Include(c => c.User);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            //ViewBag.chapter_id = new SelectList(db.Chapters, "chapter_id", "chapter_title");
            //ViewBag.user_id = new SelectList(db.Users, "user_id", "username");
            return View();
        }


        // POST: Comment/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "comment_id,chapter_id,user_id,comment_text,comment_created_at")] Comment comment)
            {
                if (ModelState.IsValid)
                {
                    var user = (TTV.Models.User)HttpContext.Session["user"];
                    int userId = user.user_id; // Lấy UserId của người dùng hiện tại
                    comment.user_id = userId;
                    comment.comment_created_at = DateTime.Now;

                    db.Comments.Add(comment);
                    db.SaveChanges();
                    
                    return Json(new { success = true, comment = comment});
                }
                return Json(new { success = false, error = "Lỗi khi thêm bình luận" });
            }


        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.chapter_id = new SelectList(db.Chapters, "chapter_id", "chapter_title", comment.chapter_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", comment.user_id);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "comment_id,chapter_id,user_id,comment_text,comment_created_at")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.chapter_id = new SelectList(db.Chapters, "chapter_id", "chapter_title", comment.chapter_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", comment.user_id);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
