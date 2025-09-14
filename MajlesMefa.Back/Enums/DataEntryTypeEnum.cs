using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums
{
    public enum DataEntryTypeEnum: short
    {
        [Display(Name = "نطق")]
        Notgh = 1,
        
        [Display(Name = "تذکر کتبی")]
        TazakorKatbi = 2,
        
        [Display(Name = "تذکر شفاهی")]
        TazakorShafahi = 3,

        [Display(Name = "مکاتبه")]
        Mokatebe = 4,

        [Display(Name = "طرح")]
        Tarh = 5,

        [Display(Name = "لایحه")]
        Layehe = 6,
        
        [Display(Name = "سوالات")]
        Soval = 7,

        [Display(Name = "اظهارات رسانه ای")]
        EzhaaratResaneee = 8,

        [Display(Name = "دستور جلسات کمیسیون")]
        DastoorJalasatComission = 9,

        [Display(Name = "تحقیق و تفحص")]
        TahghighTafahos = 10,

        [Display(Name = "ملاقات")]
        Molaghat = 11,

        [Display(Name = "تذکر")]
        Tazakor = 12, 
        
        [Display(Name = "خدمات")]
        Khadamat = 13,

        [Display(Name = "تسهیلات")]
        Loan = 14,
    }
}
