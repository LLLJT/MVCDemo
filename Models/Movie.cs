using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemo.Models
{
    public class Movie
    {
        /*
         * add validation
         * [StringLength(60,MinimumLength=3)]字段长度
         * [Required]非空
         * decimal int float DateTime[Required]
         * [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
         */
        public int Id { get; set; }
        public string Title { get; set; }
        //日期类型
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Rating { get; set; }
    }
}
