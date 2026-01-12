using AutoMapper;
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
    [ApiController]
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
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _regionRepository.GetAllAsync();
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
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateRegionRequestDTO updateRegionRequestDTO)
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
