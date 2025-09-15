using System.ComponentModel.DataAnnotations;
using Azure.Core;
using MediatR;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.UseCases.Commmands.AddUserCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateUserCommand;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MajlesMefa.UI.Models
{
    public class UserVm 
    {
        public Guid Id { get; set; }


        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(30, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Username { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "پسورد قابل قبول نیست")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        public string? Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        [Compare("Password", ErrorMessage = "پسورد و تکرار آن یکسان نمیباشد")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "پسورد قابل قبول نیست")]
        [StringLength(20, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        [Display(Name = "کلمه عبور")]
        public string EditPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        [Compare("EditPassword", ErrorMessage = "پسورد و تکرار آن یکسان نمیباشد")]
        public string EditConfirmPassword { get; set; }

        [Display(Name = "پست الکترونیکی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "{0} معتبر نیست")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Email { get; set; }

        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [RegularExpression("^09[0|1|2|3][0-9]{8}$", ErrorMessage = "{0} معتبر نیست")]
        [StringLength(11, ErrorMessage = "{0} معتبر نیست", MinimumLength = 11)]
        public string Mobile { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid? CityId { get; set; }
        public string CityName { get; set; }

        [Display(Name = "نام شرکت")]
        public Guid? OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        [Display(Name = "نام سازمان")]
        public Guid? OrganizationParentId { get; set; }
        public string OrganizationParentName { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public RoleTypeEnum Role { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int RoleInt { get { return (int)Role; } set { Role = (RoleTypeEnum)value; } }

        public bool IsActive { get; set; } = true;

    

    public AddUserCommand ConvertToCommand()
        {
            return new AddUserCommand()
            {
                Name = Name,
                Mobile = Mobile,
                Password = Password,
                Username = Username,
                CityId = CityId,
                Role = Role,
                Email = Email,
                OrganizationId= OrganizationId.HasValue ? OrganizationId : OrganizationParentId,
            };
        }
        public UpdateUserCommand ConvertToUpdateCommand()
        {
            var user = new UpdateUserCommand()
            {
                UserId = Id,
                Name = Name,
                Mobile = Mobile,
                Password = EditPassword,
                CityId = CityId,
                Role = Role,
                Email = Email,
                OrganizationId = OrganizationId.HasValue ? OrganizationId : OrganizationParentId,
            };
            return user;
        }

    }
}