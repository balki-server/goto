using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UrlShortenerModels.Models;

namespace UrlShortenerStore
{
    public class UrlShortenerStoreXML : IUrlShortenerStore
    {
        static object _lockFile = new object();
        string _xMlFilePath;
        string _backupXMLFilePath;
        string _rootElement = "ShortenedUrls";
        public UrlShortenerStoreXML(string XMLFilePath, string BackupXMLFilePath)
        {
            _xMlFilePath = XMLFilePath;
            _backupXMLFilePath = BackupXMLFilePath;
            if (!File.Exists(_xMlFilePath))
            {
                var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Url Shortener"),
                    new XElement(_rootElement));
                xDocument.Save(_xMlFilePath);
            }
        }
        public int Add(string ActualUrl, string ShortKeyword)
        {
            int maxId = -1;
            lock (_lockFile)
            {
                var xDocument = XDocument.Load(_xMlFilePath);
                try
                {
                    maxId = xDocument.Root.Elements("ShortenedUrl").Max(su => int.Parse(su.Attribute("Id").Value));
                }
                catch (InvalidOperationException)
                {
                    maxId = 0;
                }
                xDocument.Element(_rootElement).Add(
                    new XElement("ShortenedUrl", new XAttribute("Id", maxId + 1),
                    new XElement("ActualUrl", ActualUrl),
                    new XElement("CreatedDate", DateTime.Now),
                    new XElement("HitCount", 0),
                    new XElement("LastHit", null),
                    new XElement("ShortKeyword", ShortKeyword)
                    )
                    );
                xDocument.Save(_xMlFilePath);
            }
            return maxId + 1;
        }

        public int Delete(int Id)
        {
            lock (_lockFile)
            {
                var xDocument = XDocument.Load(_xMlFilePath);
                xDocument.Root.Elements("ShortenedUrl").Where(e => e.Attribute("Id").Value.Equals(Id.ToString())).Select(e => e).Single().Remove();
                xDocument.Save(_xMlFilePath);
            }
            return Id;
        }

        public void Dispose()
        {

        }

        public IEnumerable<ShortenedUrl> GetAll()
        {
            var xDocument = XDocument.Load(_xMlFilePath);
            return xDocument.Root.Elements("ShortenedUrl").Select(e => new ShortenedUrl
            {
                ActualUrl = e.Element("ActualUrl").Value,
                CreatedDate = DateTime.Parse(e.Element("CreatedDate").Value),
                HitCount = int.Parse(e.Element("HitCount").Value),
                Id = int.Parse(e.Attribute("Id").Value),
                LastHit = e.Element("LastHit").Value != "" ? (DateTime?)DateTime.Parse(e.Element("LastHit").Value) : null,
                ShortKeyword = e.Element("ShortKeyword").Value,
            });
        }

        public ShortenedUrl GetById(int Id)
        {
            var xDocument = XDocument.Load(_xMlFilePath);
            return xDocument.Root.Elements("ShortenedUrl").Where(e => e.Attribute("Id").Value.Equals(Id.ToString())).Select(e => new ShortenedUrl
            {
                ActualUrl = e.Element("ActualUrl").Value,
                CreatedDate = DateTime.Parse(e.Element("CreatedDate").Value),
                HitCount = int.Parse(e.Element("HitCount").Value),
                Id = int.Parse(e.Attribute("Id").Value),
                LastHit = e.Element("LastHit").Value != "" ? (DateTime?)DateTime.Parse(e.Element("LastHit").Value) : null,
                ShortKeyword = e.Element("ShortKeyword").Value,
            }).Single();
        }

        public int IncrementHitCount(int Id, DateTime HitTime)
        {
            int hitCount = -1;
            lock (_lockFile)
            {
                var xDocument = XDocument.Load(_xMlFilePath);
                var updatableElement = xDocument.Root.Elements("ShortenedUrl").Where(e => e.Attribute("Id").Value.Equals(Id.ToString()));
                hitCount = int.Parse(updatableElement.Select(e => e.Element("HitCount")).Single().Value);
                updatableElement.Select(e => e.Element("HitCount")).Single().SetValue(hitCount + 1);
                updatableElement.Select(e => e.Element("LastHit")).Single().SetValue(HitTime);
                xDocument.Save(_xMlFilePath);
            }
            return hitCount + 1;
        }

        public int Update(int Id, string ActualUrl, string ShortKeyword)
        {
            lock (_lockFile)
            {
                var xDocument = XDocument.Load(_xMlFilePath);
                var updatableElement = xDocument.Root.Elements("ShortenedUrl").Where(e => e.Attribute("Id").Value.Equals(Id.ToString()));
                updatableElement.Select(e => e.Element("ActualUrl")).Single().SetValue(ActualUrl);
                updatableElement.Select(e => e.Element("ShortKeyword")).Single().SetValue(ShortKeyword);
                xDocument.Save(_xMlFilePath);
            }
            return Id;
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

        public ShortenedUrl GetByKeyword(string ShortKeyword)
        {
            var xDocument = XDocument.Load(_xMlFilePath);
            return xDocument.Root.Elements("ShortenedUrl").Where(e => e.Element("ShortKeyword").Value.ToLowerInvariant().Equals(ShortKeyword.ToLowerInvariant())).Select(e => new ShortenedUrl
            {
                ActualUrl = e.Element("ActualUrl").Value,
                CreatedDate = DateTime.Parse(e.Element("CreatedDate").Value),
                HitCount = int.Parse(e.Element("HitCount").Value),
                Id = int.Parse(e.Attribute("Id").Value),
                LastHit = e.Element("LastHit").Value != "" ? (DateTime?)DateTime.Parse(e.Element("LastHit").Value) : null,
                ShortKeyword = e.Element("ShortKeyword").Value,
            }).FirstOrDefault();
        }

        public long GetTotalHitCount()
        {
            int total = 0;
            lock (_lockFile)
            {
                var xDocument = XDocument.Load(_xMlFilePath);
                try
                {
                    total = xDocument.Root.Elements("ShortenedUrl").Sum(su => int.Parse(su.Element("HitCount").Value));
                }
                catch (InvalidOperationException)
                {
                    total = 0;
                }
            }
            return total;
        }

        public bool Backup()
        {
            bool result;
            lock (_lockFile)
            {
                try
                {
                    File.Copy(_xMlFilePath, _backupXMLFilePath);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
