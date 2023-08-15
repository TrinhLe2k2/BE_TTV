using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTV.Models;
using System.Data.Entity;
using System.Text;
using System.Globalization;

namespace TTV.Controllers
{
    public class HomeController : Controller
    {
        private TangThuVienEntities db = new TangThuVienEntities();
        // GET: Home
        public ActionResult Index()
        {
            #region truyện mới cập nhật

            ViewBag.listBooks = db.Books.ToList();

            ViewBag.ListLastChapter = db.Chapters
                    .GroupBy(c => c.book_id)
                    .Select(g => g.OrderByDescending(c => c.chapter_created_at).FirstOrDefault())
                    .OrderByDescending(c => c.chapter_created_at)
                    .ToList();
            #endregion


            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        static string RemoveDiacritics(string input)
        {
            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public ActionResult Search(string searchString)
        {
            if(String.IsNullOrEmpty(searchString))
            {
                searchString = " ";
            }
            ViewBag.searchString = searchString;
            #region listLastChapters
            List<Chapter> listLastChapters = new List<Chapter>();
            foreach (var item in db.Books.ToList())
            {
                var listChapterOfBook = db.Chapters
                    .Where(c => c.book_id == item.book_id)
                    .OrderBy(c => c.chapter_number)
                    .ToList();

                var lastChapter = listChapterOfBook.LastOrDefault();
                listLastChapters.Add(lastChapter);
            }
            #endregion

            var lb = db.Books.ToList();
            List<Book> listBook = new List<Book>();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = RemoveDiacritics(searchString.ToLower());
                foreach(var item in lb)
                {
                    if (RemoveDiacritics(item.book_title.ToLower()).Contains(searchString))
                    {
                        listBook.Add(item);
                    }
                }
            }

            TempData["ListBook"] = listBook;
            TempData["ListLastChapters"] = listLastChapters;
            
            //ViewBag.listBooks = listBook;
            //ViewBag.ListLastChapters = listLastChapters;
            //ViewBag.listCategories = db.Categories.ToList();
            return RedirectToAction("ViewSearch", "Home");
        }

        [HttpGet]
        public ActionResult BoLoc(List<string> arrayId)
        {
            //arrayId.Add(idcategory.ToString());
            var listBook = db.Books.ToList();
            if(arrayId != null)
            {
                listBook = new List<Book>();
                foreach(var book in db.Books.ToList())
                {
                    List<string> array1 = new List<string>();
                    foreach(var BookCategory in db.BookCategories)
                    {
                        if(book.book_id == BookCategory.book_id)
                        {
                            array1.Add(BookCategory.category_id.ToString());
                        }
                    }
                    bool isSubset = arrayId.All(item => array1.Contains(item));
                    if(isSubset)
                    {
                        listBook.Add(book);
                    }
                }
            }

            #region listLastChapters
            List<Chapter> listLastChapters = new List<Chapter>();
            foreach (var item in db.Books.ToList())
            {
                var listChapterOfBook = db.Chapters
                    .Where(c => c.book_id == item.book_id)
                    .OrderBy(c => c.chapter_number)
                    .ToList();

                var lastChapter = listChapterOfBook.LastOrDefault();
                listLastChapters.Add(lastChapter);
            }
            #endregion
            TempData["ListBook"] = listBook;
            TempData["ListLastChapters"] = listLastChapters;

            return RedirectToAction("ViewSearch", "Home");
        }
        


        public ActionResult ViewSearch()
        {
            // Lấy danh sách từ TempData
            var listBook = TempData["ListBook"] as List<Book>;
            var listLastChapters = TempData["ListLastChapters"] as List<Chapter>;

            ViewBag.listBooks = listBook;
            ViewBag.ListLastChapters = listLastChapters;
            ViewBag.listCategories = db.Categories.ToList();
            return View();
        }


    }
}