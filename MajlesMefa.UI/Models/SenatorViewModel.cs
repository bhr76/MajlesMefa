using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.UseCases.Commmands.UpdateSenatorProfileCommand;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Enums.Senator;
using Azure.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Duende.IdentityServer.Models;
using System.Diagnostics.CodeAnalysis;

namespace MajlesMefa.UI.Models
{
    public class SenatorVm
    {
        public Guid Id { get; set; }
        public Guid UserId => Id;

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Username { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "پسورد قابل قبول نیست")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        [Compare("Password", ErrorMessage = "پسورد و تکرار آن یکسان نمیباشد")]
        public string ConfirmPassword { get; set; }

        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "پسورد قابل قبول نیست")]
        [Display(Name = "کلمه عبور")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        public string? EditPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "تعداد کاراکترهای پسورد مناسب نیست")]
        [Compare("EditPassword", ErrorMessage = "پسورد و تکرار آن یکسان نمیباشد")]
        public string? EditConfirmPassword { get; set; }

        [Display(Name = "پست الکترونیکی")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "{0} معتبر نیست")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Email { get; set; }

        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        //[RegularExpression("^09[0|1|2|3|9][0-9]{8}$", ErrorMessage = "{0} معتبر نیست")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 11)]
        public string Mobile { get; set; }

        [Display(Name = "عضویت در کمیسیون‌ها")]
        public Guid ComissionMembership { get; set; }

        [Display(Name = "عضویت در کمیسیون‌ها")]
        public string ComissionMembershipTitle { get; set; }

        //
        [Display(Name = "عضویت در فراکسیون‌ها")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string FractionMembership { get; set; }

        [Display(Name = "فعالیت‌های اجتماعی")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string SocialActivity { get; set; }

        [Display(Name = "سوابق نمایندگی")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string SenaHistory { get; set; }

        [Display(Name = "توضیح گرایش‌های سیاسی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string PoliticalTending { get; set; }

        //
        [Display(Name = "‌سابقه عضویت در هیئت رئیسه")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string SabegheHeyatReise { get; set; }

        [Display(Name = "علایق شخصی")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string PersonalFavorites { get; set; }

        //
        [Display(Name = "رشته تحصیلی")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Reshte { get; set; }

        [Display(Name = "حوزه انتخابیه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid HozeCityId { get; set; }
        public string HozeCityName { get; set; }

        [Display(Name = "شهر محل تولد")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid BirthCityId { get; set; }
        public string BirthCityName { get; set; }

        [Display(Name = "استان محل تولد")]
        public Guid? BirthCityParentId { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid CityId { get; set; }
        public string CityName { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime Birthdate { get; set; }

        //
        [Display(Name = "تاریخ تولد")]
        public string BirthdateString { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string BirthdateStr => Birthdate.ToPersianDate();

        //
        [Display(Name = "حوزه انتخابیه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public HozeEntekhabiEnum HozeEntekhabi { get; set; }

        [Display(Name = "حوزه ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int HozeEntekhabiInt { get { return (int)HozeEntekhabi; } set { HozeEntekhabi = (HozeEntekhabiEnum)value; } }

        //
        [Display(Name = "شغل غالب مردم حوزه")]
        public ShoghleGhalebEnum ShoghleGhaleb { get; set; }

        [Display(Name = "شغل غالب مردم حوزه")]
        public int ShoghleGhalebInt { get { return (int)ShoghleGhaleb; } set { ShoghleGhaleb = (ShoghleGhalebEnum)value; } }

        //
        [Display(Name = "مدرک تحصیلی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public MadrakTahsiliEnum MadrakTahsili { get; set; }

        [Display(Name = "مدرک تحصیلی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MadrakTahsiliInt { get { return (int)MadrakTahsili; } set { MadrakTahsili = (MadrakTahsiliEnum)value; } }

        //
        [Display(Name = "محل تحصیل")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public MahaleTahsilEnum MahaleTahsil { get; set; }

        [Display(Name = "محل تحصیل")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MahaleTahsilInt { get { return (int)MahaleTahsil; } set { MahaleTahsil = (MahaleTahsilEnum)value; } }

        //
        [Display(Name = "گرایش سیاسی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public GerayeshSiasiEnum GerayeshSiasi { get; set; }

        [Display(Name = "گرایش سیاسی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int GerayeshSiasiInt { get { return (int)GerayeshSiasi; } set { GerayeshSiasi = (GerayeshSiasiEnum)value; } }

        [Display(Name = "سوابق امضای استیضاح")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string SabegheEmzaEstizah { get; set; }


        [Display(Name = "سوابق قبلی")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string JobHistory { get; set; }

        [Display(Name = "عکس نماینده")]
        public IFormFile ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string ProfilePhotoStr { get; set; }


        public UpdateSenatorProfileCommand ConvertToCommand()
        {
            return new UpdateSenatorProfileCommand()
            {
                UserId = Id,
                Name = Name,
                Mobile = Mobile,
                ComissionMembership = ComissionMembership,
                SocailActivity = SocialActivity,
                PersonalFavorites = PersonalFavorites,
                BirthCityId = HozeCityId,
                HozeCityId = HozeCityId,
                JobHistory = JobHistory,
                Reshte = Reshte,
                SabegheEmzaEstizah = SabegheEmzaEstizah,
                SabegheHeyatReise = SabegheHeyatReise,
                SenaHistory = SenaHistory,
                PoliticalTending = PoliticalTending,
                CityId = CityId,
                GerayeshSiasi = GerayeshSiasi,
                FractionMembership = FractionMembership,
                BirthDate = Birthdate,
                HozeEntekhabi = HozeEntekhabi,
                MadrakTahsili = MadrakTahsili,
                MahaleTahsil = MahaleTahsil,
                ShoghleGhaleb = ShoghleGhaleb,
                ProfilePhotoFile = ProfilePhoto,
            };
        }

    }

}