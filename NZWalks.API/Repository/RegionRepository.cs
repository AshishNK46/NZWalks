using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext _dbContext;
        public RegionRepository(NZWalksDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool? isOrderByAssending = true, int PageNumber = 1, int pageSize = 1000)
        {
            var domainRegions = _dbContext.Regions.AsQueryable();
            //Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    domainRegions = domainRegions.Where(x => x.Name.Contains(filterQuery));
                }
            }
            //Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    domainRegions = isOrderByAssending ?? true ? domainRegions.OrderBy(x => x.Name) : domainRegions.OrderByDescending(x => x.Name);
                }
            }

            //Pagination
            var recordToSkip = (PageNumber - 1) * pageSize;
            return await domainRegions.Skip(recordToSkip).Take(pageSize).ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var domainRegion = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (domainRegion == null)
                return null;

            domainRegion.Code = region.Code;
            domainRegion.Name = region.Name;
            domainRegion.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return domainRegion;

        }
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var domainRegion = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (domainRegion == null) return null;

            _dbContext.Regions.Remove(domainRegion);
            await _dbContext.SaveChangesAsync();
            return domainRegion;
        }
    }
}
