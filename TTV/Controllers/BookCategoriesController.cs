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
    public class BookCategoriesController : Controller
    {
        private TangThuVienEntities db = new TangThuVienEntities();

        // GET: BookCategories
        public ActionResult Index()
        {
            var bookCategories = db.BookCategories.Include(b => b.Book).Include(b => b.Category);
            return View(bookCategories.ToList());
        }

        // GET: BookCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCategory bookCategory = db.BookCategories.Find(id);
            if (bookCategory == null)
            {
                return HttpNotFound();
            }
            return View(bookCategory);
        }

        // GET: BookCategories/Create
        public ActionResult Create()
        {

            ViewBag.categoryList = db.Categories.ToList();
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title");
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            return View();
        }

        // POST: BookCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookCategory_id,book_id,category_id")] BookCategory bookCategory, int[] selectedCategories)
        {
            //if (ModelState.IsValid)
            //{
            //    db.BookCategories.Add(bookCategory);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", bookCategory.book_id);
            //ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", bookCategory.category_id);
            //return View(bookCategory);

            if (ModelState.IsValid)
            {
                if (selectedCategories != null && selectedCategories.Length > 0)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        var newBookCategory = new BookCategory
                        {
                            book_id = bookCategory.book_id,
                            category_id = categoryId
                        };

                        db.BookCategories.Add(newBookCategory);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", bookCategory.book_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", bookCategory.category_id);
            return View(bookCategory);
        }

        // GET: BookCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCategory bookCategory = db.BookCategories.Find(id);
            if (bookCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", bookCategory.book_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", bookCategory.category_id);
            return View(bookCategory);
        }

        // POST: BookCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookCategory_id,book_id,category_id")] BookCategory bookCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_title", bookCategory.book_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", bookCategory.category_id);
            return View(bookCategory);
        }

        // GET: BookCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCategory bookCategory = db.BookCategories.Find(id);
            if (bookCategory == null)
            {
                return HttpNotFound();
            }
            return View(bookCategory);
        }

        // POST: BookCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookCategory bookCategory = db.BookCategories.Find(id);
            db.BookCategories.Remove(bookCategory);
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
