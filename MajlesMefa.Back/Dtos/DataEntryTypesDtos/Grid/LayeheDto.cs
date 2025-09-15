using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Utilities.Date;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class LayeheDto
    {
        public LayeheTarhTypeEnum Type { get; set; }
        public string TypeEnumDesc { get => this.Type.GetPersianName(); }
        
        public string ShomareSabt { get; set; }

        public DateTime ElamVosoolDate { get; set; }

        public string ElamVosoolPersianDate { get => this.ElamVosoolDate.ToPersianDate(); }

        public List<string> MajorCommissions { get; set; }

        public List<string> MinorCommissions { get; set; }

        public VazeyatBarresiEnum VazeyatBarresi { get; set; }
        public string VazeyatBarresiEnumDesc { get => this.VazeyatBarresi.GetPersianName(); }

        public DateTime EblaghDate { get; set; }
        public string EblaghPersianDate {  get => EblaghDate.ToPersianDate(); }

        public DateTime BarresiKoliatDarSahnDate { get; set; }
        public string BarresiKoliatDarSahnPersianDate { get => this.BarresiKoliatDarSahnDate.ToPersianDate();  }

        public NatijeBarresiEnum NatijeBarresiCommission { get; set; }
        public string NatijeBarresiCommissionEnumDesc { get => this.NatijeBarresiCommission.GetPersianName(); }

        public NatijeBarresiEnum NatijeBarresiSahn { get; set; }
        public string NatijeBarresiSahnEnumDesc { get => this.NatijeBarresiSahn.GetPersianName(); }

        public NatijeBarresiShoraEnum NatijeBarresiShora { get; set; }
        public string NatijeBarresiShoraEnumDesc { get => this.NatijeBarresiShora.GetPersianName();  }
    }
}
