using MajlesMefa.Back.Utilities.Date;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos
{
    public class PeygiryDto
    {
        public long Id { get; set; }
        public Guid DataEntryId { get; set; }
        public Guid PeygiriKonande { get; set; }
        public string PeygiriKonandeTitle { get; set; }
        public string PeygiriNumber { get; set; }

        public string PeygiriDescription { get; set; }

        public DateTime PeygiriDate { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        public string PeygiriDateStr => this.PeygiriDate.ToPersianDate();
    }
}
