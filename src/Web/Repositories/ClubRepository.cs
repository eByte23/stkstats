using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories
{
    public interface IClubRepository
    {
        Task<List<Club>> GetClubsAsync();
        Task<Club?> GetClubAsync(Guid? id);
        Task CreateClubAsync(Club? club);
    }


    public class InMemoryClubRepository : IClubRepository
    {
        private readonly List<Club> _clubs = new();

        public InMemoryClubRepository(List<Club> clubs)
        {
            this._clubs = clubs;
        }

        public Task<List<Club>> GetClubsAsync()
        {
            return Task.FromResult(_clubs);
        }

        public Task<Club?> GetClubAsync(Guid? id)
        {
            return Task.FromResult(_clubs.SingleOrDefault(x => x.Id == id));
        }

        public Task CreateClubAsync(Club? club)
        {
            if (club == null)
            {
                throw new ArgumentNullException(nameof(club));
            }


            _clubs.Add(club);
            return Task.CompletedTask;
        }
    }

    public class LocalStorageClubRepository : IClubRepository
    {
        private readonly RepoFileSystemStorage<Club> _repoFileSystemStorageHelper;

        public LocalStorageClubRepository(RepoFileSystemStorageHelper storageHelper)
        {
            _repoFileSystemStorageHelper = storageHelper.GetRepoFileSystemStorage<Club>();
        }

        public async Task<List<Club>> GetClubsAsync()
        {
            return await _repoFileSystemStorageHelper.GetAllAsync();
        }

        public async Task<Club?> GetClubAsync(Guid? id)
        {
            var clubs = await _repoFileSystemStorageHelper.GetAllAsync();
            return clubs.SingleOrDefault(x => x.Id == id);
        }

        public async Task CreateClubAsync(Club? club)
        {
            if (club == null)
            {
                throw new ArgumentNullException(nameof(club));
            }

            var clubs = await _repoFileSystemStorageHelper.GetAllAsync();

            clubs.Add(club);

            await _repoFileSystemStorageHelper.SaveAllAsync(clubs);
        }
    }
}