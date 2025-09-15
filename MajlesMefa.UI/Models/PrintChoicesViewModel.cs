using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class PrintChiocesVm
    {
        public Guid Id { get; set; }
        public Guid UserId => Id;

        [Display(Name = "طرح‌ و لوایح")]
        public bool HasTarh { get; set; }

        [Display(Name = "سوالات")]
        public bool HasSoal { get; set; }

        [Display(Name = "اظهارات رسانه‌ای")]
        public bool HasEzharat { get; set; }

        [Display(Name = "نطق‌ها")]
        public bool HasNotgh { get; set; }

        [Display(Name = "تحقیق و تفحص‌")]
        public bool HasTahghighTafahos { get; set; }

        [Display(Name = "‌مکاتبات")]
        public bool HasMokatebe { get; set; }

        [Display(Name = "تذکرات")]
        public bool HasTazakor { get; set; }
        [Display(Name = "همه")]
        public bool HasAllMokatebeTypes { get; set; }

        [Display(Name = "همه")]
        public bool HasAllMokatebe { get; set; }

        [Display(Name = "پی‌نوشت")]
        public bool HasPeyNeveshtMokatebe { get; set; }

        [Display(Name = "عادی")]
        public bool HasAddiMokatebe { get; set; }

        [Display(Name = "اصل 90")]
        public bool HasAsl90Mokatebe { get; set; }

        [Display(Name = "مثبت")]
        public bool IsPosetive { get; set; }

        [Display(Name = "منفی")]
        public bool IsNegative { get; set; }

        [Display(Name = "ارائه گزارش")]
        public bool IsReport { get; set; }

        [Display(Name = "بدون پاسخ")]
        public bool IsNotAnswered { get; set; }



    }

}