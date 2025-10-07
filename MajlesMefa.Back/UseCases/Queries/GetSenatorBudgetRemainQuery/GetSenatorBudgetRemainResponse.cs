using System.Runtime.InteropServices.JavaScript;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorBudgetRemainQuery
{

    public class GetSenatorBudgetRemainResponse 
    {
        public Guid SenatorBudgetId { get; set; }
        public bool IsDefined { get; set; }
        public Int64 RemainAmount { get; set; }
        public string Dsc { get; set; }
    }
}
