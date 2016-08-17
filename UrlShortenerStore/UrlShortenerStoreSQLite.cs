using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerModels.DAL;
using UrlShortenerModels.Models;

namespace UrlShortenerStore
{
    public class UrlShortenerStoreSQLite : IUrlShortenerStore
    {
        ApplicationDbContext _applicationDbContext;
        public UrlShortenerStoreSQLite()
        {
            _applicationDbContext = new ApplicationDbContext();
        }
        public int Add(string ActualUrl, string ShortKeyword)
        {
            var shortenedUrl = new UrlShortenerModels.Models.ShortenedUrl()
            {
                ActualUrl = ActualUrl,
                ShortKeyword = ShortKeyword,
                CreatedDate = DateTime.Now,
            };
            _applicationDbContext.ShortenedUrls.Add(shortenedUrl);
            _applicationDbContext.SaveChanges();
            return shortenedUrl.Id;
        }

        public bool Backup()
        {
            return _applicationDbContext.Backup();
        }

        public int Delete(int Id)
        {
            var shortenedUrl = _applicationDbContext.ShortenedUrls.Where(su => su.Id == Id).FirstOrDefault();
            if (shortenedUrl != null)
            {
                _applicationDbContext.ShortenedUrls.Remove(shortenedUrl);
                _applicationDbContext.SaveChanges();
                return shortenedUrl.Id;
            }
            return 0;
        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }

        public IEnumerable<ShortenedUrl> GetAll()
        {
            return _applicationDbContext.ShortenedUrls.ToList();
        }

        public ShortenedUrl GetById(int Id)
        {
            return _applicationDbContext.ShortenedUrls.Where(su => su.Id == Id).FirstOrDefault();
        }

        public ShortenedUrl GetByKeyword(string ShortKeyword)
        {
            return _applicationDbContext.ShortenedUrls.Where(su => su.ShortKeyword == ShortKeyword).FirstOrDefault();
        }

        public long GetTotalHitCount()
        {
            return _applicationDbContext.ShortenedUrls.Sum(su => su.HitCount);
        }

        public string HitKeyword(string ShortKeyword)
        {
            var shortenedUrl = GetByKeyword(ShortKeyword);
            if (shortenedUrl != null)
            {
                IncrementHitCount(shortenedUrl.Id, DateTime.Now);
                return shortenedUrl.ActualUrl;
            }
            return null;
        }

        public int IncrementHitCount(int Id, DateTime HitTime)
        {
            var shortenedUrl = _applicationDbContext.ShortenedUrls.Where(su => su.Id == Id).FirstOrDefault();
            if (shortenedUrl != null)
            {
                shortenedUrl.HitCount++;
                shortenedUrl.LastHit = HitTime;
                _applicationDbContext.SaveChanges();
                return shortenedUrl.HitCount;
            }
            return 0;
        }

        public int Update(int Id, string ActualUrl, string ShortKeyword)
        {
            var shortenedUrl = _applicationDbContext.ShortenedUrls.Where(su => su.Id == Id).FirstOrDefault();
            if (shortenedUrl != null)
            {
                shortenedUrl.ActualUrl = ActualUrl;
                shortenedUrl.ShortKeyword = ShortKeyword;
                _applicationDbContext.SaveChanges();
                return shortenedUrl.Id;
            }
            return 0;
        }

    }
}
