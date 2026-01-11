using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalksRepository walksRepository;
        private readonly IMapper mapper;

        public WalksController(IWalksRepository walksRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var domainWalks = await walksRepository.GetAllAsync();
            return Ok(mapper.Map<List<WalkDTO>>(domainWalks));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var domainWalk = await walksRepository.GetByIdAsync(id);
            if (domainWalk == null)
                return NotFound();

            return Ok(mapper.Map<WalkDTO>(domainWalk));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalkDTO createWalkDTO)
        {
            var domainWalk = await walksRepository.CreateAsync(mapper.Map<Walk>(createWalkDTO));
            return CreatedAtAction(nameof(GetById), new { id = domainWalk.Id }, mapper.Map<WalkDTO>(domainWalk));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var domainWalk = await walksRepository.UpdateAsync(id, mapper.Map<Walk>(updateWalkDTO));
            if (domainWalk == null) return NotFound();

            return Ok(mapper.Map<UpdateWalkDTO>(domainWalk));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var domainWalk = await walksRepository.DeleteAsync(id);
            if (domainWalk == null) return NotFound();

            return Ok(mapper.Map<WalkDTO>(domainWalk));
        }
    }
}
