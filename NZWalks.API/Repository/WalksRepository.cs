using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDBContext dBContext;

        public WalksRepository(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<List<Walk>> GetAllAsync()
        {
            return await dBContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).ToListAsync();
        }
        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dBContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(w => w.Id == id);
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dBContext.Walks.AddAsync(walk);
            await dBContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkToUpdate = await dBContext.Walks.FirstOrDefaultAsync(w => w.Id == id);
            if (walkToUpdate == null) return null;

            walkToUpdate.Name = walk.Name;
            walkToUpdate.Code = walk.Code;
            walkToUpdate.LengthInKm = walk.LengthInKm;
            walkToUpdate.WalkImageUrl = walk.WalkImageUrl;
            walkToUpdate.DifficultyId = walk.DifficultyId;
            walkToUpdate.RegionId = walk.RegionId;
            await dBContext.SaveChangesAsync();
            return walkToUpdate;

        }
        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var currentWalk = await dBContext.Walks.FirstOrDefaultAsync(w => w.Id == id);
            if (currentWalk == null) return null;

            dBContext.Walks.Remove(currentWalk);
            await dBContext.SaveChangesAsync();
            return currentWalk;
        }
    }
}
