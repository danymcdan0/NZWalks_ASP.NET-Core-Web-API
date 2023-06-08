using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public ImagesController(IMapper mapper, IImageRepository imageRepository)
        {
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        //POST: /api/images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO imageUploadRequestDTO) 
        {
            ValidateFileUpload(imageUploadRequestDTO);

            if (ModelState.IsValid)
            {
                //Convert DTO to Domain Model
                //var domainImageUpload = mapper.Map<Image>(imageUploadRequestDTO);
                var domainImageUpload = new Image
                {
                    File = imageUploadRequestDTO.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDTO.File.FileName),
                    FileSizeInBytes = imageUploadRequestDTO.File.Length,
                    FileName = imageUploadRequestDTO.FileName,
                    FileDescription = imageUploadRequestDTO.FileDescription
                };

                //User repository to upload image
                await imageRepository.Upload(domainImageUpload);

                return Ok(domainImageUpload);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO imageUploadRequestDTO)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(imageUploadRequestDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (imageUploadRequestDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size > 10MB - Too large");
            }
        }
    }
}
