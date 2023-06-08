using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "100 Characters max")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "1000 Characters max")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Between 0 and 50 Only")]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
