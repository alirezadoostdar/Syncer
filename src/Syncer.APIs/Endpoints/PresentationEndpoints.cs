using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Syncer.APIs.Models.Domain;
using Syncer.APIs.Persistence;
using System.Xml.Linq;

namespace Syncer.APIs.Endpoints;

public static class PresentationEndpoints
{
    public static void MapPresentationEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var group = endpoint.MapGroup("presentation").WithTags("Presentation");

        group.MapPost("/{speaker}",
             async (string spaker, CreatePresentationRequest request, SyncerDbContext dbContext) =>
        {
            try
            {
                var presentation = Presentation.Create(request.UnifiedId, request.title, request.descriotion, spaker);
                dbContext.Presentations.Add(presentation);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/{presentation_id}/Milestone",
     async ([FromRoute(Name = "presentation_id")] string presentationId,
     CreateMilestoneRequest request, SyncerDbContext dbContext, IMemoryCache cache) =>
     {
         try
         {
             var presentation = await dbContext.Presentations.Include(z => z.Milestones).FirstAsync(x => x.Id == presentationId);

             if (!request.AllowedEmojis.All(d => cache.TryGetValue(d, out _)))
                 return Results.BadRequest("invalid emojies!");
             var milestone = Milestone.Create(request.title, request.descriotion, request.AllowedEmojis);
             presentation.AddMilestone(milestone);
             await dbContext.SaveChangesAsync();
             return Results.Ok();
         }
         catch (Exception ex)
         {
             return Results.BadRequest(ex.Message);
         }
     });


        group.MapPut("/{presentation_id}/present",
            async ([FromRoute(Name = "presentation_id")] string presentationId,
            SyncerDbContext dbContext,
            IMemoryCache cache) =>
            {
                try
                {
                    var presentation = await dbContext.Presentations.FirstAsync(x => x.Id == presentationId);

                    presentation.StartPresent();
                    await dbContext.SaveChangesAsync();

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

        group.MapPost("/{presentation_id}/join",
                    async ([FromRoute(Name = "presentation_id")] string presentationId,
                    CreateJoinRequest request,
                    SyncerDbContext dbContext,
                    IMemoryCache cache) =>
{
    try
    {
        var presentation = await dbContext.Presentations.Include(z => z.Joiners).FirstAsync(x => x.Id == presentationId);

        presentation.AddJoiner(new PresentationJoiner(request.joinerId));
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
    }
}

public record CreatePresentationRequest(string UnifiedId, string title, string descriotion);
public record CreateMilestoneRequest(string title, string descriotion, List<string> AllowedEmojis);
public record CreateJoinRequest(string joinerId);

