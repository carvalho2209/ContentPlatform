namespace Contracts;

public record GetArticleViewsResponse
{
    public Guid Id { get; set; }
    
    public long Views { get; set; }
}