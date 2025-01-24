using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



    namespace DataAnalysis.Model
    {
        public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
        {
            public void Configure(EntityTypeBuilder<Product> builder)
            {
                builder.HasKey(e => e.Id)
                       .HasName("PK_Product");

                builder.ToTable("Product");

                builder.Property(e => e.Id).HasColumnName("id");
                builder.Property(e => e.BrandNameEn).HasColumnName("brand_name_en");
                builder.Property(e => e.BrandNameFa).HasColumnName("brand_name_fa");
                builder.Property(e => e.CategoryKeywords).HasColumnName("category_keywords");
                builder.Property(e => e.CategoryTitleFa).HasColumnName("category_title_fa");
                builder.Property(e => e.ProductTitleEn).HasColumnName("product_title_en");
                builder.Property(e => e.ProductTitleFa).HasColumnName("product_title_fa");
                builder.Property(e => e.TitleAlt).HasColumnName("title_alt");
                builder.Property(e => e.UrlCode).HasColumnName("url_code");
            }
        }
    }


