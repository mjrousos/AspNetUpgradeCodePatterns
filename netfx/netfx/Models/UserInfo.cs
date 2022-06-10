using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netfx.Models;

public class UserInfo
{
    public string UserName { get; set; }
    public string UniqueId { get; set; }
    public string UserAgent { get; set; }
    public int IncrementCount { get; set; }
    public string CurrentUrl { get; set; }
    public string ProfilePictureUrl { get; set; }
}