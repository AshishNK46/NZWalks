namespace NZWalks.API.Models.DTO
{
    public class CreateWalkDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
