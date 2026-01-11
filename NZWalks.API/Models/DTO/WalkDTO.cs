using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        //public Guid DifficultyId { get; set; }
        //public Guid RegionId { get; set; }

        //Navigation Property
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
