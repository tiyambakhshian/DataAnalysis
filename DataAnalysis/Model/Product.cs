using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAnalysis.Model;

[Table("Product")]
public partial class Product
{

    [Column("id")]
    [Key]
    public int Id { get; set; }


    [Column("product_title_fa")]
    [StringLength(255)]
    public string? ProductTitleFa { get; set; }

    [Column("product_title_en")]
    [StringLength(255)]
    public string? ProductTitleEn { get; set; }

    [Column("url_code")]
    [StringLength(100)]
    public string? UrlCode { get; set; }

    [Column("title_alt")]
    [StringLength(255)]
    public string? TitleAlt { get; set; }
    
    [Column("category_title_fa")]
    [StringLength(255)]
    public string? CategoryTitleFa { get; set; }

    [Column("category_keywords")]
    [StringLength(255)]
    public string? CategoryKeywords { get; set; }

    [Column("brand_name_fa")]
    [StringLength(255)]
    public string? BrandNameFa { get; set; }

    [Column("brand_name_en")]
    [StringLength(255)]
    public string? BrandNameEn { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }  

}