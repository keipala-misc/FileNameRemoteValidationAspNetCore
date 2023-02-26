using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FileNameRemoteValidationAspNetCore.Pages;

public class IndexModel : PageModel
{
    [Required]
    [PdfFileType(ErrorMessage = "Please select a PDF file.")]
    public List<IFormFile> Files { get; set; }

    [PageRemote(
        ErrorMessage = "Name must end with .pdf",
        HttpMethod = "POST",
        AdditionalFields = "__RequestVerificationToken",
        PageHandler = "CheckFileNames"
    )]
    [BindProperty]
    //[FileName(ErrorMessage = "Name must end with .pdf")]
    public string FileNames { get; set; }

    public IndexModel()
    {
    }

    public void OnGet()
    {
    }

    public JsonResult OnPostCheckFileNames()
    {
        if (FileNames.ToLower().EndsWith(".pdf"))
        {
            return new JsonResult("String ends with .pdf");
        }
        return new JsonResult(true);
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

// removed because I don't think I can use it for my purposes
//public class FileNameAttribute : ValidationAttribute
//{
//    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//    {
//        if (value is string fileNamesString)
//        {
//            var fileNames = fileNamesString.Split("/");
//            foreach (var name in fileNames)
//            {
//                if (name.ToLower().EndsWith(".pdf"))
//                {
//                    return new ValidationResult(ErrorMessage);
//                }
//            }
//        }
//        return ValidationResult.Success;
//    }
//}