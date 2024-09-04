using System;
using System.Collections.Generic;

namespace Alset_Research.Models;

public partial class Follower
{
    public int Id { get; set; }

    public int ResearcherId { get; set; }

    public int FollowerId { get; set; }

    public virtual User FollowerNavigation { get; set; } = null!;

    public virtual User Researcher { get; set; } = null!;
}
