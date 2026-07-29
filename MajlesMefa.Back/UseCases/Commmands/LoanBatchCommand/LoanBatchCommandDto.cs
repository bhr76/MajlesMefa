
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;

namespace MajlesMefa.Back.UseCases.Commmands.LoanBatchCommand
{
    public class BatchBaseDto
    {
        public bool AllSelected { get; set; }
        public List<Guid> SelectedIds { get; set; } = new List<Guid>();
        public TableRequestModel Filter { get; set; } // استفاده از مدل ساختاریافته به جای string
        public Guid? SenatorId { get; set; } // اضافه شدن فیلتر نماینده در صورت اعمال در گرید
    }

    public class BatchChangeStatusDto : BatchBaseDto
    {
        public int Status { get; set; }
    }

    public class BatchRegisterActionDto : BatchBaseDto
    {
        public int ActionType { get; set; }
        public string Description { get; set; }
    }
}
