using System;
using System.Collections.Generic;
using DataAnalysis.Model;

namespace DataAnalysis;

public partial class Comment
{
    public int? ProductId { get; set; }

    public string? ConfirmedAt { get; set; }

    public string? Comment1 { get; set; }

    public virtual Product? Product { get; set; }
}
