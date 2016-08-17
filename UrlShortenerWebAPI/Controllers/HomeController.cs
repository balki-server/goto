using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using UrlShortenerStore;

namespace UrlShortenerWebAPI.Controllers
{
    public class HomeController : Controller
    {
        IUrlShortenerStore _store;
        public RedirectResult Index(string id)
        {   
            _store = new UrlShortenerStoreFactory().Create();
            var actualUrl = _store.HitKeyword(id);
            var totalHitCount = _store.GetTotalHitCount();

            if (actualUrl == null)
                return Redirect("http://google.com");
            else
            {
                var totalHitCountForBackup = long.Parse(WebConfigurationManager.AppSettings["HitCountForBackup"]);
                if (totalHitCount % totalHitCountForBackup == 0)
                {
                    var result = _store.Backup();
                }
                return Redirect(actualUrl);
            }
        }
        public ActionResult ShortenUrl()
        {
            ViewBag.urlShortenerWebAPIURL = WebConfigurationManager.AppSettings["UrlShortenerWebAPIURL"];
            return View();
        }
        public ActionResult InvalidKeyword()
        {
            return View();
        }
    }
}
