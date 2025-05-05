using System.ComponentModel.DataAnnotations;

namespace HomeTasksApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "İsim alanı gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Display(Name = "Yeni bir household oluştur")]
        public bool IsCreatingNewHousehold { get; set; }

        [Required(ErrorMessage = "Household adı gereklidir.")]
        [Display(Name = "Household Adı")]
        public string? HouseholdName { get; set; }
    }
}
