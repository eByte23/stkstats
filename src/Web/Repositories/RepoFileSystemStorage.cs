using System.Text.Json;

namespace STKBC.Stats.Repositories
{

    public class RepoFileSystemStorageHelper
    {
        private readonly string storePath;

        public RepoFileSystemStorageHelper(string storePath)
        {
            this.storePath = storePath;
        }

        internal bool CreateStoreFile(string name)
        {
            var path = Path.Combine(storePath, "Data" , "Repositories", name);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
                return true;
            }

            return true;
        }

        internal string GetStoreFilePath(string name)
        {
            return Path.Combine(storePath, "Data", "Repositories", name);
        }

        public RepoFileSystemStorage<T> GetRepoFileSystemStorage<T>() where T : class
        {
            return new RepoFileSystemStorage<T>(this);
        }
    }

    public class RepoFileSystemStorage<T> where T : class
    {
        private readonly string _storeName;
        private RepoFileSystemStorageHelper repoFileSystemStorageHelper;

        public RepoFileSystemStorage(RepoFileSystemStorageHelper repoFileSystemStorageHelper)
        {
            this.repoFileSystemStorageHelper = repoFileSystemStorageHelper;
            _storeName = typeof(T).Name;


            repoFileSystemStorageHelper.CreateStoreFile(_storeName);
        }

        public async Task<List<T>> GetAllAsync()
        {
            
            var path = repoFileSystemStorageHelper.GetStoreFilePath(_storeName);
            var json = await File.ReadAllTextAsync(path);
            var list = JsonSerializer.Deserialize<List<T>>(json);
            return list ?? new List<T>();
        }


        public async Task SaveAllAsync(List<T> list, Action<List<T>>? postSaveUpdate = null)
        {
            var path = repoFileSystemStorageHelper.GetStoreFilePath(_storeName);
            var json = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(path, json);

            postSaveUpdate?.Invoke(list);
        }


    }
}