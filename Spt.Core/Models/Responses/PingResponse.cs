namespace Spt.Core.Models;

public class PingResponse : ISptResponse<string>
{
    public string Response { get; set; } = "";
}
