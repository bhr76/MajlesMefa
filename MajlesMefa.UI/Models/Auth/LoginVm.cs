using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models.Auth
{
    public class LoginVm
    {
        [MaxLength(5)]
        [Required(ErrorMessage = "پر کردن این فیلد اجباری است")]
        public string CaptchaCode { get; set; }

        [Required(ErrorMessage = "پر کردن این فیلد اجباری است")]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required(ErrorMessage = "پر کردن این فیلد اجباری است")]
        [MaxLength(20)]
        public string Password { get; set; }
    }


    public class LoginApiVm
    {

        [Required(ErrorMessage = "پر کردن این فیلد اجباری است")]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required(ErrorMessage = "پر کردن این فیلد اجباری است")]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
