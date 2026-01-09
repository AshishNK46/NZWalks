using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
//using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private NZWalksDBContext _dbContext;
        public RegionsController(NZWalksDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region> {
            //     new Region()
            //     {
            //         Id=Guid.NewGuid(),
            //         Name="Auckland Region",
            //         Code="AKL",
            //         RegionImageUrl="",
            //     },
            //     new Region()
            //     {
            //         Id=Guid.NewGuid(),
            //         Name="Wellington Region",
            //         Code="WLG",
            //         RegionImageUrl="",
            //     }
            //};
            var regionsDomain = _dbContext.Regions.ToList();

            var regionsDTO = new List<RegionsDTO>();
            foreach (var region in regionsDomain)
            {
                regionsDTO.Add(new RegionsDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionsDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = _dbContext.Regions.Find(id);
            var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (region == null)
                return NotFound();

            var regionsDTO = new RegionsDTO()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionsDTO);
        }
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionDTO)
        {
            var domainRegion = new Region()
            {
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImageUrl = addRegionDTO.RegionImageUrl
            };

            _dbContext.Regions.Add(domainRegion);
            _dbContext.SaveChanges();


            var regionsDTO = new RegionsDTO()
            {
                Id = domainRegion.Id,
                Code = domainRegion.Code,
                Name = domainRegion.Name,
                RegionImageUrl = domainRegion.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = domainRegion.Id }, regionsDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var domainRegion = _dbContext.Regions.FirstOrDefault(s => s.Id == id);
            if (domainRegion == null)
            {
                return NotFound();
            }
            domainRegion.Code = updateRegionRequestDTO.Code;
            domainRegion.Name = updateRegionRequestDTO.Name;
            domainRegion.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            _dbContext.SaveChanges();

            var regionDTO = new RegionsDTO()
            {
                Id = domainRegion.Id,
                Code = domainRegion.Code,
                Name = domainRegion.Name,
                RegionImageUrl = domainRegion.RegionImageUrl
            };
            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var domainRegion = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (domainRegion == null)
            {
                return NotFound();
            }
            _dbContext.Remove(domainRegion);
            _dbContext.SaveChanges();

            var regionDTO = new RegionsDTO()
            {
                Id = domainRegion.Id,
                Code = domainRegion.Code,
                Name = domainRegion.Name,
                RegionImageUrl = domainRegion.RegionImageUrl
            };
            return Ok(regionDTO);
        }
    }
}
