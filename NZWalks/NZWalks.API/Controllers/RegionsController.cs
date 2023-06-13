using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper) 
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //GET ALL REGIONS
        //GET https://localhost:1234/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() 
        {
            // Get data from database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var regionsDTO = mapper.Map<List<RegionDTO>>(regionsDomain);

            //Return DTOs
            return Ok(regionsDTO);
        }

        //GET SINGLE REGION (Get Region by ID)
        //Get: https://localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            //Get Region Domain model from Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            //var region = dbContext.Regions.Find(id); [ALT]

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Region Domain Model to Region DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomain);

            return Ok(regionDTO);
        }

        //POST to Create New Region
        //POST: https://localhost:1234/api/regions
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO) 
        {
            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

            //Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            //Returning the newly created Region with a GET (GetById)
            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        //Update region
        //PUT: https://localhost:1234/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO) 
        {
            //Map DTO to Domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

            //Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDTO);
        }

        //Delete region
        //DELETE: https://localhost:1234/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            //Find region
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Return deleted region back
            //Map domain to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDTO);
        }
    }
}
