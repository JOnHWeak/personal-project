using personal_project.Helper;
using System.ComponentModel.DataAnnotations;

namespace personal_project.DTO
{
    public class TitleDTO
    {
        public class TitleForm
        {
            [Required]
            [TitleIdFormat(ErrorMessage = "Title ID must be in the format: 2 uppercase letters followed by 4 digits.")]
            public string title_id { get; set; }
            [Required]
            [StringLength(80, ErrorMessage = "Title cannot exceed 80 characters.")]
            public string title { get; set; }
            
            [StringLength(12, ErrorMessage = "Type cannot exceed 12 characters.")]
            public string? type { get; set; }

            public string pub_id { get; set; }
            public decimal? price { get; set; }
            public decimal? advance { get; set; }
            public int? royalty { get; set; }
            public int? ytd_sales { get; set; }
            public string notes { get; set; }
            public DateTime pubdate { get; set; }
        }
        public class UpdateTitle
        {
            
            [StringLength(80, ErrorMessage = "Title cannot exceed 80 characters.")]
            public string title { get; set; }

            [StringLength(12, ErrorMessage = "Type cannot exceed 12 characters.")]
            public string? type { get; set; }

            public string pub_id { get; set; }
            public decimal? price { get; set; }
            public decimal? advance { get; set; }
            public int? royalty { get; set; }
            public int? ytd_sales { get; set; }
            public string notes { get; set; }
            public DateTime pubdate { get; set; }
        }
    }
}
