using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FileNameRemoteValidationAspNetCore.Pages;

public class IndexModel : PageModel
{
    [Required]
    [PdfFileType(ErrorMessage = "Please select a PDF file.")]
    public List<IFormFile> Files { get; set; }

    public IndexModel()
    {
    }

    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            return Page();
        }
        return Page();
    }
}

public class PdfFileTypeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var files = value as List<IFormFile>;
        if (files != null)
        {
            foreach (var file in files)
            {
                if (file.ContentType != "application/pdf")
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
        }
        return ValidationResult.Success;
    }
}