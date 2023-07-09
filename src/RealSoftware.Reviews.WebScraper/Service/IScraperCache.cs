using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealSoftware.Reviews.WebScraper.Service
{
    public interface IScraperCache
    {
        ///     
        ///
        ///
        Task<ValueTuple<CacheInfoModel, TData>> Get<TData>(string key,/*Max Age in hours. (default 1 week = 168) */ int maxAge = 168);
        Task<CacheInfoModel> List(string type,/*Max Age in hours. (default 1 week = 168) */ int maxAge = 168);
        Task<Guid> Save<TData>(string key, string type, TData dataFileContent, string htmlFileContent, string subFolder = null, MemoryStream ms = null);
        Task<string> LoadHtmlFile(CacheInfoModel cacheitem);
    }

    public class ScraperCache : IScraperCache, IDisposable
    {
        private static JsonSerializerOptions Json_options = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
        };
        const string CACHE_DATA_FILE = "cache.json";
        public string CacheStorePath { get; }
        public string CacheFilePath { get; }
        public string Type { get; }

        // public int UnsavedItems { get; set; } = 0;

        private Dictionary<string, CacheInfoModel> _store = new Dictionary<string, CacheInfoModel>();
        private bool disposedValue;

        public ScraperCache(string cacheStorePath, string type = "default")
        {
            Type = type;
            CacheStorePath = cacheStorePath;

            CreateCacheFolder(CacheStorePath);
            CacheFilePath = Path.Combine(cacheStorePath, CACHE_DATA_FILE);
            LoadStore(CacheFilePath);
        }

        private void LoadStore(string dataFile)
        {
            if (!File.Exists(dataFile))
            {
                File.WriteAllText(dataFile, "{}");
            }
            else
            {
                var reader = JsonSerializer.Deserialize<CacheStore>(File.ReadAllText(dataFile), Json_options);
                _store = reader.Items.ToDictionary(x => x.Key);
            }
        }

        private void SaveStore(string dataFile)
        {
            lock (_store)
            {
                var store = new CacheStore
                {
                    Items = _store.Values.ToList()
                };
                var storeJson = JsonSerializer.Serialize<CacheStore>(store, Json_options);
                File.WriteAllText(dataFile, storeJson);
            }
        }

        public async Task<ValueTuple<CacheInfoModel, TData>> Get<TData>(string key, int maxAge = 168)
        {
            if (!_store.ContainsKey(key)) return new ValueTuple<CacheInfoModel, TData>();

            var info = _store[key];

            var dataFilePath = Path.Combine(GetCacheItemPath(info), info.DataFile);
            string dataText = await File.ReadAllTextAsync(dataFilePath);
            var data = JsonSerializer.Deserialize<TData>(dataText);

            return (info, data);
        }

        public Task<CacheInfoModel> List(string type, int maxAge = 168)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Save<T>(string key, string type, T data, string htmlFileContent, string subFolder = null, MemoryStream ms = null)
        {
            Guid id = Guid.NewGuid();
            var item = _store.ContainsKey(key) ? _store[key] : new CacheInfoModel
            {
                Id = id,
                Key = key,
                Type = type,
                DataFile = $"{id}.json",
                HtmlFile = $"{id}.html",
                SubFolder = subFolder,
                CachedAt = DateTimeOffset.Now
            };

            string cacheItemPath = GetCacheItemPath(item);
            var dataFile = Path.Combine(cacheItemPath, item.DataFile);
            var htmlFile = Path.Combine(cacheItemPath, item.HtmlFile);
            var screenshotFile = Path.Combine(cacheItemPath, $"screenshot.jpg");

            if (_store.ContainsKey(key))
            {
                File.Delete(dataFile);
                File.Delete(htmlFile);
                File.Delete(screenshotFile);
            }

            var storeJson = JsonSerializer.Serialize(data, Json_options);
            await File.WriteAllTextAsync(dataFile, storeJson);
            await File.WriteAllTextAsync(htmlFile, htmlFileContent);

            if (ms != null)
            {
                string screenshot = screenshotFile;
                using (var fs = new FileStream(screenshot, FileMode.OpenOrCreate))
                {
                    ms.Position = 0;
                    await ms.CopyToAsync(fs);
                    await ms.FlushAsync();
                    item.ScreenshotFile = screenshot;
                }
            }


            _store[item.Key] = item;
            // UnsavedItems++;

            // if (UnsavedItems > 10)
            // {
            SaveStore(CacheFilePath);
            // UnsavedItems = 0;
            // }

            return item.Id;
        }

        private string GetCacheItemPath(CacheInfoModel item)
        {
            var baseCacheItemPath = Path.Combine(CacheStorePath, item.Type ?? Type);
            CreateCacheFolder(baseCacheItemPath);
            string cacheItemPath = null;
            if (!string.IsNullOrWhiteSpace(item.SubFolder))
            {
                cacheItemPath = Path.Combine(baseCacheItemPath, item.SubFolder, item.Id.ToString());
            }
            else
            {
                cacheItemPath = Path.Combine(baseCacheItemPath, item.Id.ToString());
            }

            CreateCacheFolder(cacheItemPath);

            return cacheItemPath;
        }

        internal void CreateCacheFolder(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SaveStore(CacheFilePath);
                    _store = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Task<string> LoadHtmlFile(CacheInfoModel cacheitem)
        {
            string cacheItemPath = GetCacheItemPath(cacheitem);

            var htmlFile = Path.Combine(cacheItemPath, cacheitem.HtmlFile);

            return File.ReadAllTextAsync(htmlFile);
        }
    }

    public class CacheStore
    {
        public int Count => Items?.Count ?? 0;
        public List<CacheInfoModel> Items { get; set; } = new List<CacheInfoModel>();
    }


    public class CacheInfoModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string SubFolder { get; set; }
        public string DataFile { get; set; }
        public string HtmlFile { get; set; }
        public string ScreenshotFile { get; set; }
        public DateTimeOffset CachedAt { get; set; }
    }
}