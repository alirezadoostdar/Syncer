
using System.Reflection;

namespace Syncer.APIs.Models.Domain;

public class Presentation
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Speaker { get; set; }
    public PresentationStatus Status { get; set; }
    public ICollection<PresentationJoiner> Joiners { get; set; }
    public ICollection<Milestone> Milestones { get; set; } = null!;

    public static Presentation Create(string id,string titlte,string description, string speaker)
    {
        return new Presentation
        {
            Description = description,
            Id = id,
            Title = titlte,
            Speaker = speaker,
            Status = PresentationStatus.Create
        };
    }

    internal void AddMilestone(Milestone milestone)
    {
        if (Status != PresentationStatus.Create)
            throw new Exception("invalid presentaion status!");
        Milestones ??= [];
        Milestones.Add(milestone);
    }

    internal void AddJoiner(PresentationJoiner joiner)
    {
        if (Status != PresentationStatus.Present)
            throw new Exception("invalid presentaion status!");
        Joiners ??= [];
        Joiners.Add(joiner);
    }

    internal void StartPresent()
    {
        if (Status != PresentationStatus.Create)
            throw new Exception("invalid presentaion status!");
        Status = PresentationStatus.Present;
    }
}

public enum PresentationStatus
{
    Create = 1,
    Present = 2,
    Finished = 3
}


public record PresentationJoiner(string Name);