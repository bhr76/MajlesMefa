using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Enums.Senator;

namespace MajlesMefa.Back.Dtos
{
    public class SenatorBudgetDto
    {
        public Guid Id { get; set; }

        public Guid SenatorId { get; set; }

        public string SenatorName { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public long Amount { get; set; }

        public string ExecDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public SenatorRequestLoanTypeEnum SenatorRequestLoanType { get; set; }

        public string LoanTypeName { get; set; }
    }

    public class CurrentSenatorBudgetDto
    {
        public Guid Id { get; set; }

        public Int64 Amount { get; set; }
    }
}