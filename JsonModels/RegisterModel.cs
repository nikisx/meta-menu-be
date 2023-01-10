using System.ComponentModel.DataAnnotations;

namespace meta_menu_be.JsonModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
    }
}
