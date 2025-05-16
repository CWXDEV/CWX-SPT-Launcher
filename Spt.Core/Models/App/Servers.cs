using System.ComponentModel.DataAnnotations;

namespace Spt.Core.Models;

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

    public bool Locked
    {
        get;
        set;
    }
}
