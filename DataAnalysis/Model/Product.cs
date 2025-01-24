using System;
using System.Collections.Generic;

namespace DataAnalysis.Model;

public partial class Product
{
    public int Id { get; set; }

    public string? ProductTitleFa { get; set; }

    public string? ProductTitleEn { get; set; }

    public string? UrlCode { get; set; }

    public string? TitleAlt { get; set; }

    public string? CategoryTitleFa { get; set; }

    public string? CategoryKeywords { get; set; }

    public string? BrandNameFa { get; set; }

    public string? BrandNameEn { get; set; }
}
