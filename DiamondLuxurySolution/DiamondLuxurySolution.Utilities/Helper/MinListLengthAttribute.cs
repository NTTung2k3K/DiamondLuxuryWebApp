using System.ComponentModel.DataAnnotations;
using System.Collections;
namespace DiamondLuxurySolution.Utilities.Helper
{
    public class MinListLengthAttribute : ValidationAttribute
    {
        private readonly int _minLength;

        public MinListLengthAttribute(int minLength)
        {
            _minLength = minLength;
            ErrorMessage = $"Danh sách phải chứa ít nhất {_minLength} phần tử.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IList list && list.Count >= _minLength)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
