using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "code should have minimum 3 length")]
        [MaxLength(3, ErrorMessage = "code should have maximum 3 length")]
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
