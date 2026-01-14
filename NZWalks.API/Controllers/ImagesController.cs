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
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;

        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] UploadImageDTO uploadImageDTO)
        {
            ValidateImage(uploadImageDTO);
            if (ModelState.IsValid)
            {
                var imageDomain = new Image()
                {
                    File = uploadImageDTO.File,
                    FileName = uploadImageDTO.FileName,
                    FileDescription = uploadImageDTO.FileDescription,
                    FileExtention = Path.GetExtension(uploadImageDTO.File.FileName),
                    FileSizeInBytes = uploadImageDTO.File.Length
                };
                imageDomain = await imageRepository.Upload(imageDomain);

                return Ok(imageDomain);
            }
            return BadRequest(ModelState);
        }

        private void ValidateImage(UploadImageDTO uploadImageDTO)
        {
            var acceptedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!acceptedFileExtensions.Contains(Path.GetExtension(uploadImageDTO.File.FileName)))
            {
                ModelState.AddModelError("File", "Unsupported File Extension");
            }
            if (uploadImageDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("File", "File Size should be less then 10MB");
            }
        }
    }
}
