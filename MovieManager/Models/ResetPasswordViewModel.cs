using MovieManager.Data;
using System.ComponentModel.DataAnnotations;

namespace MovieManager.Models;

public class ResetPasswordViewModel
{
    [Display(Name = "E-Posta")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta yazınız!")]
    public string? UserName { get; set; }



}
