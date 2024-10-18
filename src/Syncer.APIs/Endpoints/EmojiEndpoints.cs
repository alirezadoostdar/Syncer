using Syncer.APIs.Models.Domain;
using Syncer.APIs.Persistence;

namespace Syncer.APIs.Endpoints;

public static class EmojiEndpoints
{
    public static void MapEmojiEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var group = endpoint.MapGroup("emojies").WithTags("Emoji");

        group.MapPost("/", static async (string code, string name, SyncerDbContext dbContext) =>
        {
            try
            {
                dbContext.Emojis.Add(Emoji.Create(code, name));
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
