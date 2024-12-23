using AustraliyaDemo.Context;
using AustraliyaDemo.Entity;
using AustraliyaDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PagedList;
using System.Drawing;
using System.Globalization;
using System.IO;
using PagedList.Mvc;
using System.Net.Http.Headers;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace AustraliyaDemo.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly DbMyContext _dbContext;
        private readonly IWebHostEnvironment _Iweb;


        public AdminController(DbMyContext DbContext, IWebHostEnvironment iweb)
        {
            _dbContext = DbContext;
            _Iweb = iweb;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult AddSite(string usname)
        {
            ViewBag.Id = usname;
            var st = _dbContext.SiteTypes.ToList();
            var country = _dbContext.Countries.ToList();
            ViewBag.Country = country;
            ViewBag.SityTypes = st;

            return View();

        }

        public IActionResult AddCountry(string countryname)
        {
            if (ModelState.IsValid)
            {
                Country objcountry = new Country();
                objcountry.CountryName = countryname;
                using (var context = _dbContext)
                {
                    context.Countries.Add(objcountry);
                    context.SaveChanges();
                    return RedirectToAction("AddSite");
                }
            }
            return RedirectToAction("AddSite");
        }
        public IActionResult AddSiteType(string name)
        {
            if (ModelState.IsValid)
            {
                SiteType objtype = new SiteType();
                objtype.SiteName = name;
                using (var context = _dbContext)
                {
                    context.SiteTypes.Add(objtype);
                    context.SaveChanges();
                    return RedirectToAction("AddSite");
                }
            }

            return RedirectToAction("AddSite");
        }
        [HttpPost]
        public async Task<IActionResult> AddSite(Site objsite, IFormCollection files)
        {
            var filename = "";

            foreach (var file in files.Files)
            {
                if (files.Files != null)
                {
                    filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    objsite.LogoPath = filename;
                    var filenamePath = _Iweb.WebRootPath + $@"\SiteImages\{filename}";
                    using (FileStream fs = System.IO.File.Create(filenamePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                }
            }
            objsite.LogoPath = filename;
            objsite.IsActive = true;
            if (filename != null)
            {
                using (var context = _dbContext)
                {

                    context.Sites.Add(objsite);
                    context.SaveChanges();
                }

            }
            return RedirectToAction("ShowSite");
        }
        /* public List<Site> Dosort(List<Site> sites, string Sortproperty, SortOrder sortOrder)
         {

             if (Sortproperty.ToLower() == "name")
             {
                 if (sortOrder == SortOrder.Ascending)
                     sites = sites.OrderBy(n => n.Name).ToList();
                 else
                     sites = sites.OrderByDescending(n => n.Name).ToList();
             }
             else
             {
                 if (sortOrder == SortOrder.Ascending)
                     sites = sites.OrderBy(d => d.Url).ToList();
                 else
                     sites = sites.OrderByDescending(d => d.Url).ToList();
             }
             return sites;
         }
 */
        public IActionResult ShowSite(string usname, string searchText = "", int pg = 1)
        {
            ViewBag.Id = usname;
            List<Site> sites;
            if (searchText != "" && searchText != null)
            {
                sites = _dbContext.Sites.Where(s => s.Name.Contains(searchText) || s.Url.Contains(searchText)).ToList();
            }
            else
                sites = _dbContext.Sites.Where(x => x.IsActive == true).ToList();

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int rescount = sites.Count();
            var pager = new PagerModel(rescount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = sites.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);


        }
        public IActionResult DeleteSite(int? id)
        {
            try
            {
                var site = _dbContext.Sites.Where(x => x.Id == id).FirstOrDefault();
                 var article =_dbContext.Articles.Where(x => x.SiteId == id).FirstOrDefault();
                 
                if (article != null)
                {
                    article.IsActive = false;
                    _dbContext.Update(article);
                    _dbContext.SaveChanges();
                }
                if (site != null)
                {
                    site.IsActive = false;
                    _dbContext.Update(site);
                    _dbContext.SaveChanges();                  
                }
            
                return RedirectToAction("ShowSite");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IActionResult EditSite(int id)
        {
            var country = _dbContext.Countries.ToList();
            var st = _dbContext.SiteTypes.ToList();
            ViewBag.SityTypes = st;
            ViewBag.Country = country;
            var site = _dbContext.Sites.Find(id);
            return View(site);
        }
        [HttpPost]
        public IActionResult EditSite(Site objsite, IFormCollection files)
        {
            var filename = "";

            foreach (var file in files.Files)
            {
                if (files.Files != null)
                {
                    filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var filenamePath = _Iweb.WebRootPath + $@"\SiteImages\{filename}";
                    using (FileStream fs = System.IO.File.Create(filenamePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                }
            }
            objsite.LogoPath = filename;
            if (filename != null)
            {

                var exist = _dbContext.Sites.Where(x => x.Id == objsite.Id).FirstOrDefault();
                if (exist != null)
                {
                    exist.Name = objsite.Name;
                    exist.CountryId = objsite.CountryId;
                    exist.Url = objsite.Url;
                    exist.LogoPath = objsite.LogoPath;
                    exist.TypeId = objsite.TypeId;
                    _dbContext.Sites.Update(exist);
                    _dbContext.SaveChanges();
                }
            }

            return RedirectToAction("ShowSite");

        }
        [HttpPost]
        public JsonResult ViewRocord(int id)
        {
            var site = _dbContext.Sites.Where(x => x.Id == id).FirstOrDefault();

            return Json(site);

        }
      
        public IActionResult AddArticle(string usname)
        {
            ViewBag.Id = usname;
            var country = _dbContext.Countries.ToList();
            ViewBag.Country = country;
            var st = _dbContext.SiteTypes.ToList();
            ViewBag.SityTypes = st;
            var sitename = _dbContext.Sites.ToList();
            ViewBag.Sites = sitename;
            var articletype = _dbContext.ArticleTypes.ToList();
            ViewBag.ArticleType = articletype;
            return View();
        }

        public IActionResult AddArticleType(string articlename)
        {
            if (ModelState.IsValid)
            {
                ArticleType article = new ArticleType();
                article.ArticleName = articlename;
                using (var context = _dbContext)
                {
                    context.ArticleTypes.Add(article);
                    context.SaveChanges();
                    return RedirectToAction("AddArticle");
                }
            }
            return RedirectToAction("AddArticle");
        }
        [HttpPost]
        public IActionResult AddArticle(Article Objarticle)
        {
            Objarticle.IsActive=true;
            using (var context = _dbContext)
            {
                context.Articles.Add(Objarticle);
                context.SaveChanges();
            }

            return View();
        }
        public IActionResult ShowArticle(string usname, string searchText = "", int pg = 1)
        {
            ViewBag.Id = usname;
            List<Article> articles;
            if (searchText != "" && searchText != null)
            {
                articles = _dbContext.Articles.Where(s => s.ArticleName.Contains(searchText) || s.Tags.Contains(searchText)).ToList();
            }
            else
                articles = _dbContext.Articles.Where(x => x.IsActive == true).ToList();
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int rescount = articles.Count();
            var pager = new PagerModel(rescount, pg, pageSize);
            int recskip = (pg - 1) * pageSize;
            var data = articles.Skip(recskip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);

        }
        public IActionResult DeleteArticle(int? id)
        {
            try
            {
                var article = _dbContext.Articles.Where(x => x.Id == id).FirstOrDefault();
                var commen = _dbContext.Comments.Where(x => x.ArticleId == id).FirstOrDefault();

                if (commen != null)
                {
                    commen.IsActive = false;
                    _dbContext.Update(commen);
                    _dbContext.SaveChanges();
                }
                if (article != null)
                {
                    article.IsActive = false;
                    _dbContext.Articles.Update(article);
                    _dbContext.SaveChanges();
                }
                return RedirectToAction("ShowArticle");
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public IActionResult EditArticle(int id)
        {
            var sitename = _dbContext.Sites.ToList();
            var arttype = _dbContext.ArticleTypes.ToList();
            ViewBag.Site = sitename;
            ViewBag.ArticleType = arttype;
            var art = _dbContext.Articles.Find(id);
            return View(art);
        }
        [HttpPost]
        public IActionResult EditArticle(Article objarticle)
        {
            if (ModelState.IsValid)
            {
                var exist = _dbContext.Articles.Where(x => x.Id == objarticle.Id).FirstOrDefault();
                if (exist != null)
                {
                    exist.SiteId = objarticle.SiteId;
                    exist.ArticleName = objarticle.ArticleName;
                    exist.URL = objarticle.URL;
                    exist.Decription = objarticle.Decription;
                    exist.TypeId = objarticle.TypeId;
                    exist.Tags = objarticle.Tags;
                    _dbContext.Update(exist);
                    _dbContext.SaveChanges();
                }
            }
            return RedirectToAction("ShowArticle");
        }
        [HttpPost]
        public JsonResult GetRocordArticle(int id)
        {
            var art = _dbContext.Articles.Where(x => x.Id == id).FirstOrDefault();

            return Json(art);

        }
       
        public IActionResult ShowComment(string usname)
        {
            ViewBag.Id = usname;
            var coment = GetComment();
            return View(coment);


        }
        public IEnumerable<CommentModel> GetComment()
        {
            try
            {
                var list = (from c in _dbContext.Comments // outer sequence
                            join ar in _dbContext.Articles on c.ArticleId equals ar.Id // key selector 
                            where c.IsActive==true && ar.IsActive==true
                            select new CommentModel
                            { // result selector 
                                Id = c.Id,
                                ArticleId = c.ArticleId,
                                ArticleName = ar.ArticleName,
                                Description = ar.Decription,
                                Commenttext = c.Comments,


                            }).AsQueryable();
                return list;
            }
            catch (Exception ex) 
            {
                return null;
            }

        }
        public IActionResult DeleteComment(int? id)
        {
            try
            {
                var comment = _dbContext.Comments.Where(x => x.Id == id).FirstOrDefault();
                if (comment != null)
                {
                    comment.IsActive = false;
                    _dbContext.Comments.Update(comment);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex){ }
            return RedirectToAction("ShowComment");
        }
        public IActionResult ShowReport(string usname)
        {
            ViewBag.Id = usname;
            var report = GetReport();
            return View(report);
        }
        public IEnumerable<ReporViewModel> GetReport()
        {
            try
            {
                var Query = (from r in _dbContext.ReportComments.ToList()
                             join c in _dbContext.Comments.ToList() on r.CommentId equals c.Id
                             where r.IsActive == true && c.IsActive == true
                             select new ReporViewModel
                             {
                                 ReportId = r.Id,
                                 CommentId = c.Id,
                                 CommentName = c.Comments,
                                 ReportName = r.ReportComments
                             }).AsQueryable();
                return Query;
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        public IActionResult DeleteReport(int? id)
        {
            try
            {
                var report = _dbContext.ReportComments.Where(x => x.Id == id).FirstOrDefault();
                if (report != null)
                {
                    report.IsActive = false;
                    _dbContext.ReportComments.Update(report);
                    _dbContext.SaveChanges();
                    
                }
            }
            catch (Exception ex){ }
            return RedirectToAction("ShowReport");
        }
    }
}
