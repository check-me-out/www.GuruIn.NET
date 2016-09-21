namespace WebFramework.Models.Helpers
{
    using System.ComponentModel.DataAnnotations;

    public class CheckMyNameAttribute : ValidationAttribute
    {
        public CheckMyNameAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueToCheck = value != null ? value.ToString() : string.Empty;
            if (valueToCheck.ToLower().Contains("guru"))
            {
                return new ValidationResult("Sorry, Guru is my name and taken already.");//this.FormatErrorMessage(validationContext.DisplayName)
            }

            return ValidationResult.Success;
        }
    }
}
