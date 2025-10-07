using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class SenatorBudgetRepository : BaseRepository<SenatorBudgetEntity>, ISenatorBudgetRepository
    {
        public SenatorBudgetRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public async Task<bool> HasDefineBudget(Guid senatorId, Guid userId)
        {
            var result = await _context.SenatorBudgets.AnyAsync(x => x.SenatorId == senatorId && x.UserId == userId);
            return result;
        }
        public async Task<CurrentSenatorBudgetDto> GetDefineBudget(Guid senatorId, Guid userId)
        {
            var result = await _context.SenatorBudgets.Where(x => x.SenatorId == senatorId && x.UserId == userId).OrderByDescending(x => x.ExecDate).Select(x => new { x.Amount, x.Id }).FirstAsync();
            return new CurrentSenatorBudgetDto() { Id = result.Id, Amount = result.Amount };
        }
    }
}