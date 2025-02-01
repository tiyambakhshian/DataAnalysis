using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Model
{
    public partial class Comment
    {

        [Column("product_id")]
        [Key]
        public int ProductId { get; set; }


        [Column("confirmed_at")]
        [StringLength(255)]
        public string? ConfirmedAt { get; set; }

        [Column("comment")]
        [StringLength(255)]
        public string? Comments { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
}
