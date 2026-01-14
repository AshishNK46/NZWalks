using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
//using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    //[Authorize(Roles = "Reader")]
    public class RegionsController : ControllerBase
    {

        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? IsSortedByAssending, int pageNumber = 1, int pageSize = 1000)
        {
            var regionsDomain = await _regionRepository.GetAllAsync(filterOn, filterQuery, sortBy, IsSortedByAssending, pageNumber, pageSize);
            return Ok(_mapper.Map<List<RegionsDTO>>(regionsDomain));
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
                return NotFound();

            return Ok(_mapper.Map<RegionsDTO>(region));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionDTO)
        {
            var domainRegion = _mapper.Map<Region>(addRegionDTO);
            domainRegion = await _regionRepository.CreateAsync(domainRegion);
            return CreatedAtAction(nameof(GetById), new { id = domainRegion.Id }, _mapper.Map<RegionsDTO>(domainRegion));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var domainRegion = await _regionRepository.UpdateAsync(id, _mapper.Map<Region>(updateRegionRequestDTO));
            if (domainRegion == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RegionsDTO>(domainRegion));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var domainRegion = await _regionRepository.DeleteAsync(id);
            if (domainRegion == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RegionsDTO>(domainRegion));
        }
    }
}
