using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerModels.Models
{
    public class ShortenedUrl
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string ActualUrl { get; set; }
        [MaxLength(25)]
        public string ShortKeyword { get; set; }
        public DateTime CreatedDate { get; set; }
        public int HitCount { get; set; }
        public DateTime? LastHit { get; set; }
    }
}
