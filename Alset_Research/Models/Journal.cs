using System;
using System.Collections.Generic;

namespace Alset_Research.Models;

public partial class Journal
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly PublicationDate { get; set; }

    public string Pdffile { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
