using System.ComponentModel.DataAnnotations;

namespace personal_project.DTO
{
    public class StoreDTO
    {
        public class StoreForm
        {
            [Required]
            [StringLength(4, MinimumLength = 4, ErrorMessage = "StoreID must be exactly 4 characters.")]
            [RegularExpression(@"^\d{4}$", ErrorMessage = "StoreID must be exactly 4 numeric characters.")]
            public string stor_id { get; set; }

            public string stor_name { get; set; }

            public string stor_address { get; set; }

            public string city { get; set; }

            [StringLength(2, ErrorMessage = "State must be exactly 2 uppercase characters.")]
            [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "State must be exactly 2 uppercase characters.")]
            public string state { get; set; }

            [StringLength(5, ErrorMessage = "ZIP code cannot exceed 5 characters.")]
            public string zip { get; set; }
        }

        public class UpdateStore
        {
            public string stor_name { get; set; }

            public string stor_address { get; set; }

            public string city { get; set; }

            [StringLength(2, ErrorMessage = "State must be exactly 2 uppercase characters.")]
            [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "State must be exactly 2 uppercase characters.")]
            public string state { get; set; }

            [StringLength(5, ErrorMessage = "ZIP code cannot exceed 5 characters.")]
            public string zip { get; set; }
        }
    }
}
