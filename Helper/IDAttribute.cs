using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace personal_project.Helper
{
    public class IDAttribute : ValidationAttribute
    {
        private static readonly Regex _regex = new Regex(@"^[A-Z]{2}\d{4}$", RegexOptions.Compiled);

        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                return _regex.IsMatch(str);
            }
            return false;
        }
    }
    public class EmpIdFormatAttribute : ValidationAttribute
    {
        private static readonly Regex _regex = new Regex(@"^[A-Z]{3}[1-9]\d{4}[FM]$|^[A-Z]-[A-Z][1-9]\d{4}[FM]$", RegexOptions.Compiled);

        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                return _regex.IsMatch(str);
            }
            return false;
        }
    }
}
