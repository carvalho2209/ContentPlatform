using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Api.Database;
using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Features.Articles;

public class GetArticleViewsConsumer : IConsumer<GetArticleViewsRequest>
{
    private readonly ApplicationDbContext _context;

    public GetArticleViewsConsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<GetArticleViewsRequest> context)
    {
        if (!await _context.Articles.AnyAsync(x => x.Id == context.Message.Id))
        {
            await context.RespondAsync(new ArticleNotFoundResponse { Id = context.Message.Id });

            return;
        }

        var views = await _context
            .ArticleEvents
            .Where(x => x.EventType == ArticleEventType.View
                        && x.ArticleId == context.Message.Id)
            .CountAsync();

        var response = new GetArticleViewsResponse
        {
            Id = context.Message.Id,
            Views = views
        };

        await context.RespondAsync(response);
    }
}