using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "3 Characters Min")]
        [MaxLength(3, ErrorMessage = "3 Characters Max")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "100 Characters Max")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
