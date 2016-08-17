using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using System.Web.OData.Query;
using UrlShortenerModels.Models;
using UrlShortenerStore;

namespace UrlShortenerWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", PreflightMaxAge = 120)]    
    public class ShortenedUrlsController : ODataController
    {
        IUrlShortenerStore _store;
        public ShortenedUrlsController()
        {
            _store = new UrlShortenerStoreFactory().Create();
        }
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult GetShortenedUrls()
        {
            return Ok(_store.GetAll());
        }
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult GetShortenedUrl([FromODataUri] int key)
        {
            return Ok(_store.GetById(key));
        }        
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult PostShortenedUrl(ShortenedUrl shortenedUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_store.Add(shortenedUrl.ActualUrl, shortenedUrl.ShortKeyword));
        }
        [AcceptVerbs("PATCH", "MERGE")]
        [Authorize(Roles ="Admin")]
        public IHttpActionResult Patch([FromODataUri] int key, ShortenedUrl shortenedUrl)
        {
            return Ok(_store.Update(key, shortenedUrl.ActualUrl, shortenedUrl.ShortKeyword));
        }
        [AcceptVerbs("DELETE")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            return Ok(_store.Delete(key));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _store.Dispose();
            base.Dispose(disposing);
        }
    }
}
