using System;
using System.Collections.Generic;

namespace FixIt.Models;

public partial class Review
{
    public int RId { get; set; }

    public int SId { get; set; }

    public int UId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual SparePart SIdNavigation { get; set; } = null!;

    public virtual Users UIdNavigation { get; set; } = null!;
}
