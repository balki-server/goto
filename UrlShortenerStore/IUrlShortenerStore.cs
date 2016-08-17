using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerModels.Models;

namespace UrlShortenerStore
{
    public interface IUrlShortenerStore : IDisposable
    {
        IEnumerable<ShortenedUrl> GetAll();
        ShortenedUrl GetById(int Id);
        int Delete(int Id);
        int Add(string ActualUrl, string ShortKeyword);
        int Update(int Id, string ActualUrl, string ShortKeyword);
        int IncrementHitCount(int Id, DateTime HitTime);
        string HitKeyword(string ShortKeyword);
        ShortenedUrl GetByKeyword(string ShortKeyword);
        long GetTotalHitCount();
        bool Backup();
    }
}
