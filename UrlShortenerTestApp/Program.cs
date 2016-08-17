using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            UrlShortenerStore.IUrlShortenerStore store = new UrlShortenerStore.UrlShortenerStoreSQLite();
            int id = store.Add("abc", "def");
            store.Update(id, "def", "abc");
            store.IncrementHitCount(id, DateTime.Now.AddDays(1));
            var obj = store.GetById(id);
            var objs = store.GetAll();
            store.Delete(id);

            //store = new UrlShortenerStore.UrlShortenerStoreXML("test.xml", );
            
        }
    }
}
