using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UrlShortenerStore
{
    public class UrlShortenerStoreFactory
    {
        public IUrlShortenerStore Create()
        {
            var storeType = ConfigurationManager.AppSettings["StoreType"];
            switch (storeType.ToLower())
            {
                case "xml":
                    return new UrlShortenerStoreXML(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["XMLFilePath"]),
                        HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["BackupXMLFilePath"]));
                    break;
                case "sqlite":
                    return new UrlShortenerStoreSQLite();
                    break;
                default:
                    throw new NotImplementedException(string.Format("Store type {0} not implemented.", storeType));
            }
        }
    }
}
