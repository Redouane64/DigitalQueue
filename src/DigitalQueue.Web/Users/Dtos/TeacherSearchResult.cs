namespace DigitalQueue.Web.Users.Dtos;

public class TeacherSearchResult
{
    public TeacherSearchResult(IEnumerable<object> results, object pagination)
    {
        Results = results;
        Pagination = pagination;
    }

    public IEnumerable<object> Results { get; }

    public object Pagination { get; }
}
