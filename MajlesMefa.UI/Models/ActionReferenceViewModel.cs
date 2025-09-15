using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class ActionReferenceViewModel
    {
        public Guid DataEntryId { get; set; }
        //[Required(ErrorMessage = "{0} را انتخاب نمایید."), Display(Name = "اداره کل")]
        //public Guid OrganizationId { get; set; }
        //public SelectList OrganizationSelectList { get; set; }
        //[Required(ErrorMessage = "{0} را انتخاب نمایید."), Display(Name = "شهر")]
        //public Guid SubCityId { get; set; }
        //public SelectList SubCitySelectList { get; set; }
        //[Required(ErrorMessage = "{0} را انتخاب نمایید."), Display(Name = "شرکت زیر مجموعه")]
        //public Guid SubOrganizationId { get; set; }
        //public SelectList SubOrganizationSelectList { get; set; }
        //[Required(ErrorMessage = "{0} را انتخاب نمایید."), Display(Name = "استان")]
        //public Guid CityId { get; set; }
        //public SelectList CitySelectList { get; set; }
        [Required(ErrorMessage = "{0} را انتخاب نمایید."), Display(Name = "کاربر")]
        public Guid UserId { get; set; }
        public SelectList UserSelectList { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نوع ارجاع")]
        public RefTypeEnum RefType { get; set; }
    }
    public class ActionViewModel
    {
        public Guid DataEntryId { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نوع اقدام")]
        public ActRefTypeEnum ActRefType { get; set; }
    }
}
