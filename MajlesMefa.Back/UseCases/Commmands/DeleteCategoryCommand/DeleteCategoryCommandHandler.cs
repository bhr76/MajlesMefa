using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteCategoryCommand
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CategoryRepository.Tracking
                .Include(x => x.Categories)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Categories)
                .Where(x => !x.DataEntries.Any())
                .Where(x => !x.Categories.Any(y => y.DataEntries.Any()))
                .Where(x => !x.Categories.Any(y => y.Categories.Any(z => z.DataEntries.Any())))
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);
            if(entity == null)
            {
                var count = await _unitOfWork.CategoryRepository.Tracking
                .Include(x => x.Categories)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Categories)
                .Where(x => x.Id == request.Id)
                .Select(x =>
                    x.DataEntries.Count
                    + x.Categories.Sum(y => y.DataEntries.Count)
                    + x.Categories.Sum(y => y.Categories.Sum(z => z.DataEntries.Count))
                )
                .SingleOrDefaultAsync(cancellationToken);
                if(count > 0)
                {
                    throw new InvalidOperationException($"تعداد {count} دیتا انتری به دسته بندی انتخابی و زیر دسته ها متصل می باشد");
                }
                else
                {
                    throw new NullReferenceException("دسته بندی یافت نشد");
                }
            }
            foreach(var item in entity.Categories)
            {
                foreach(var inner in item.Categories)
                {
                    _unitOfWork.CategoryRepository.Delete(inner);
                }
                _unitOfWork.CategoryRepository.Delete(item);
            }
            _unitOfWork.CategoryRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
