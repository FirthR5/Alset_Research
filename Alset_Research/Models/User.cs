using System;
using System.Collections.Generic;

namespace Alset_Research.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Follower> FollowerFollowerNavigations { get; set; } = new List<Follower>();

    public virtual ICollection<Follower> FollowerResearchers { get; set; } = new List<Follower>();

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}
