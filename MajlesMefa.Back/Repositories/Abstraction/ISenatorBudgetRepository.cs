using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Dtos;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface ISenatorBudgetRepository : IRepository<SenatorBudgetEntity>
    {
        public Task<bool> HasDefineBudget(Guid senatorId, Guid userId);
        public  Task<CurrentSenatorBudgetDto> GetDefineBudget(Guid senatorId, Guid userId);
    }
}