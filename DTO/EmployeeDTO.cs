using personal_project.Helper;
using System.ComponentModel.DataAnnotations;

namespace personal_project.DTO
{
    public class EmployeeDTO
    {
        public class EmployeeForm
        {
            [Required]
            [EmpIdFormat(ErrorMessage = "emp_id format is incorrect. Must follow [A-Z][A-Z][A-Z][1-9][0-9][0-9][0-9][0-9][FM] or [A-Z]-[A-Z][1-9][0-9][0-9][0-9][0-9][FM].")]
            public string emp_id { get; set; }

            public string fname { get; set; }
            public string minit { get; set; }
            public string lname { get; set; }
            public short job_id { get; set; }
            public byte? job_lvl { get; set; }
            public string pub_id { get; set; }
            public DateTime hire_date { get; set; }
        }
        public class UpdateEmployee
        {
            public string fname { get; set; }
            public string minit { get; set; }
            public string lname { get; set; }
            public short job_id { get; set; }
            public byte? job_lvl { get; set; }
            public string pub_id { get; set; }
            public DateTime hire_date { get; set; }
        }
    }
}
