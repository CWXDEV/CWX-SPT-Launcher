using System.ComponentModel.DataAnnotations;

namespace Spt.Core.App;

public class Servers
{
    [Required]
    public string Ip
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string ServerId
    {
        get;
        set;
    }
}
