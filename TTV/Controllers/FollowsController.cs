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
    public class FollowsController : Controller
    {
        private TangThuVienEntities db = new TangThuVienEntities();

        [HttpPost]
        public ActionResult FollowStory(int storyId)
        {
            var user = (TTV.Models.User)HttpContext.Session["user"];
            int userId = user.user_id; // Lấy UserId của người dùng hiện tại

            var isFollowing = db.Follows.FirstOrDefault(f => f.book_id == storyId && f.user_id == userId);

            if (isFollowing != null)
            {
                ViewBag.isFollowing = true;
                // Nếu đang theo dõi, xóa lượt theo dõi từ CSDL
                var userFollow = db.Follows.FirstOrDefault(uf => uf.user_id == userId && uf.book_id == storyId);
                if (userFollow != null)
                {
                    db.Follows.Remove(userFollow);
                    db.SaveChanges();
                }
            }
            else
            {
                ViewBag.isFollowing = false;
                // Nếu chưa theo dõi, thêm lượt theo dõi vào CSDL
                var userFollow = new Models.Follow
                {
                    user_id = userId,
                    book_id = storyId,
                    follow_created_at = DateTime.Now
                };
                db.Follows.Add(userFollow);
                db.SaveChanges();
            }

            ViewBag.countFollow = db.Follows.Count(f=>f.book_id == storyId);

            // Trả về kết quả JSON
            return Json(new { success = true });
        }

        // GET: Follows
        public ActionResult Index()
        {
            var follows = db.Follows.Include(f => f.Book).Include(f => f.User);
            return View(follows.ToList());
        }

        // GET: Follows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        // GET: Follows/Create
        public ActionResult Create()
        {
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title");
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username");
            return View();
        }

        // POST: Follows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "follow_id,book_id,user_id,follow_created_at")] Follow follow)
        {
            if (ModelState.IsValid)
            {
                db.Follows.Add(follow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", follow.book_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", follow.user_id);
            return View(follow);
        }

        // GET: Follows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", follow.book_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", follow.user_id);
            return View(follow);
        }

        // POST: Follows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "follow_id,book_id,user_id,follow_created_at")] Follow follow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(follow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", follow.book_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", follow.user_id);
            return View(follow);
        }

        // GET: Follows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Follow follow = db.Follows.Find(id);
            db.Follows.Remove(follow);
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
