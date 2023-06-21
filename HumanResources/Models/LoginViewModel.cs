using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class LoginViewModel
    {
        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "{0} alaný boþ býrakýlamaz!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta yazýnýz!")]
        public string? UserName { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "{0} alaný boþ býrakýlamaz!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Oturum Açýk Kalsýn")]
        public bool IsPersistent { get; set; }

        public string? ReturnUrl { get; set; }

    }
}