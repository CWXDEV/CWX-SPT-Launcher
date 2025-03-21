namespace Spt.Core.Responses;

public interface ISptResponse<T>
{
    public T Response
    {
        get;
        set;
    }
}
