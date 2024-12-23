using AustraliyaDemo.Context;
using AustraliyaDemo.Entity;
using AustraliyaDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AustraliyaDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbMyContext _dbContext;
        private readonly IWebHostEnvironment _Iweb;

        public HomeController(DbMyContext DbContext, IWebHostEnvironment iweb)
        {
            _dbContext = DbContext;
            _Iweb = iweb;

        }

        public IActionResult Index( string searchText = "")
        {
            
            var result = GetAllArticle();
           
            return View(result);
            

        }
        public IEnumerable<ArticleViewModel> GetAllArticle()
        {
           
            var list = (from art in _dbContext.Articles.ToList() // outer sequence
                        join st in _dbContext.Sites.ToList() on art.SiteId equals st.Id
                        where art.IsActive==true && st.IsActive==true// key selector 
                        select new ArticleViewModel
                        { // result selector 
                            Id = art.Id,
                            ArticleName = art.ArticleName,
                            Description = art.Decription,
                            URL = art.URL,
                            LogoPath = st.LogoPath,
                        }).AsQueryable();
            return list;

        }

        [HttpPost]
        public IActionResult Comment(Comment objcomment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objcomment.IsActive = true;
                    using (var context = _dbContext)
                    {

                        context.Comments.Add(objcomment);
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }
        [HttpPost]
        public JsonResult GetCommentId(int id)
        {
            ViewBag.Id=id;  
            if (ModelState.IsValid)
            {
                var comment = _dbContext.Comments.Where(x => x.ArticleId == id && x.IsActive==true).ToList();
                return Json(comment);
            }

            return Json("Index");
        }
        [HttpPost]
        public IActionResult ReportComment(ReportComment objreport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objreport.IsActive = true;
                    using (var context = _dbContext)
                    {
                        context.ReportComments.Add(objreport);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
