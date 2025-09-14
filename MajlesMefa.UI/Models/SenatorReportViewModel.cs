using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;

namespace MajlesMefa.UI.Models
{
    public class SenatorReportVm
    {
        public SenatorVm InitialData { get; set; }
        public List<MokatebeVm> Mkatebe { get; set;}
        public DataSourceResult Layehe { get; set;}
        public List<TarhViewModel> Tarh { get; set;}
        public List<SoalVm> Soal { get; set;}
        public List<EzharatVm> Ezharat { get; set;}
        public List<NotghViewModel> Notgh { get; set;}
        public List<TahghighTafahosVm> TahghighTafahos { get; set;}
        public List<TazakorViewModel> Tazakorat { get; set;}

    }

}