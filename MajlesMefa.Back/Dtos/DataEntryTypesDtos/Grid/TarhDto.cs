using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class TarhDto
    {
        public Guid DataEntryId { get; set; }

        public string Shenase { get; set; }

        public NatijeBarresiEnum NatijeBarresi { get; set; }
        public string NatijeBarresiEnumDesc { get => this.NatijeBarresi.GetPersianName(); }

        public VazeyatBarresiEnum VazeyatBarresi { get; set; }
        public string VazeyatBarresiEnumDesc { get => this.VazeyatBarresi.GetPersianName(); }

        public NatijeBarresiEnum Kollyat { get; set; }
        public string KollyatEnumDesc { get => this.Kollyat.GetPersianName(); }

        public NatijeBarresiEnum MavadTarh { get; set; }
        public string MavadTarhEnumDesc { get => this.MavadTarh.GetPersianName(); }

        public NatijeBarresiEnum NatijeBarresiShoraNegahban { get; set; }
        public string NatijeBarresiShoraNegahbanEnumDesc { get => this.NatijeBarresiShoraNegahban.GetPersianName(); }
        public string NatijeBarresiShoraNegahbanDescription { get; set; }

        public string SavabeghEblagh { get; set; }

        public string GhozarshNahayi { get; set; }

        public string Havashi { get; set; }

        public string ErsalBeVazir { get; set; }
        public string HamahangiBaraSherkat { get; set; }

        public string NamayandeghanBaraSherkat { get; set; }
        public string NazarNamayande { get; set; }
        public string GozareshMozakerat { get; set; }
        public Guid RelatedComission { get; set; }

        public string Gardeshkar { get; set; }
        public string NamayandeghanEmzaKonande { get; set; }
    }
}
