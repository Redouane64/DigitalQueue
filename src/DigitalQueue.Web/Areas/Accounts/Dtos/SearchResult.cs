namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class SearchResult<T>
{
    public SearchResult(IEnumerable<T> results)
        : this(results, new SearchPaginaion())
    {
        
    }
    
    public SearchResult(IEnumerable<T> results, SearchPaginaion pagination)
    {
        Results = results;
        Pagination = pagination;
    }

    public IEnumerable<T> Results { get; }

    public SearchPaginaion Pagination { get; }

    public record SearchPaginaion(bool More = false);

}
