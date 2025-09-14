using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums
{
    public enum ActRefTypeEnum: short
    {
        [Display(Name = "ارجاع دادن")]
        Refer = 1,

        [Display(Name = "سایر اقدام")]
        Action = 2,

        [Display(Name = "تایید")]
        Confirm = 3,

        [Display(Name = "عدم تایید")]
        Deny = 4,
        

        
        //sys
        [Display(Name = "ایجاد")]
        Create = 5,
        
        //وزارت خونه
        [Display(Name = "بستن")]
        Close = 6,

        //sys
        [Display(Name = "ویرایش")]
        Edit = 7,

        [Display(Name = "ارسال برای وزیر و واحد های تابعه")]
        SendToVazir = 8,

        [Display(Name = "دریافت پاسخ")]
        Answer = 9,
    }
}
