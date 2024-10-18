namespace Syncer.APIs.Models.Domain;

public class Presentation
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Speaker { get; set; }
    public ICollection<Milestone> Milestones { get; set; } = null!;
}
