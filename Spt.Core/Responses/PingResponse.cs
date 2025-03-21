namespace Spt.Core.Responses;

public class PingResponse : ISptResponse<string>
{
    public string Response
    {
        get;
        set;
    } = "";
}
