using System.ComponentModel.DataAnnotations;

namespace BTL.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpge" };
                
                bool result = extensions.Any(x=>extension.EndsWith(x));
                if(!result)
                {
                    return new ValidationResult("Allow extensions are jpg, png or jpge");
                }
            }
            return ValidationResult.Success;
        }
    }
}
