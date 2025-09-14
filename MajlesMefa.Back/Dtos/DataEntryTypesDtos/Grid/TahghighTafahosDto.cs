using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.TahghighTafahos;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class TahghighTafahosDto
    {
        public Guid Id { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Tarikh { get; set; }

        [Display(Name = "تاریخ")]
        public string JalaliTarikh => this.Tarikh.ToPersianDate();

        [Display(Name = "شماره نامه")]
        public string ShomareName { get; set; }

        [Display(Name = "شماره دریافت")]
        public string ShomareDaryaft { get; set; }

        [Display(Name = "کمیسیون مربوطه")]
        public Guid Commission { get; set; }

        [Display(Name = "سازمان/معاونت مربوطه")]
        public string Organization { get; set; }

        [Display(Name = "مخاطب")]
        public string Mokhatab { get; set; }

        [Display(Name = "نمایندگان")]
        public List<TahghighTafahosSenatorEntity> Senators { get; set; }

        [Display(Name = "وضعیت")]
        public TahghighTafahosVazyatEnum TahghighTafahosVazyat { get; set; }
        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }

        [Display(Name = "وضعیت")]
        public EnumDto<TahghighTafahosVazyatEnum> TahghighTafahosVazyatDesc => new EnumDto<TahghighTafahosVazyatEnum>
        {
            Value = this.TahghighTafahosVazyat,
            Name = this.TahghighTafahosVazyat.GetPersianName()
        };
    }
}
