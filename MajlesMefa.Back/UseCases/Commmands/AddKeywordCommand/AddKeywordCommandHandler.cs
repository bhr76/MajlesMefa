using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddKeywordCommand
{
    public class AddKeywordCommandHandler : IRequestHandler<AddKeywordCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddKeywordCommandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddKeywordCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.KeywordRepository
                .Tracking
                .Where(x => x.DataEntryId == request.DataEntryId)
                .Where(x => x.Name == request.Name)
                .SingleOrDefaultAsync(cancellationToken);
            if (entity != null)
            {
                if (!request.AddRepeatIfExist)
                {
                    throw new InvalidOperationException("امکان ایجاد کلید واژه تکراری برای موجودیت یکسان وجود ندارد");
                }
                entity.RepeatCount += request.RepeatCount;
            }
            else
            {
                entity = _mapper.Map<KeywordEntity>(request);
                _unitOfWork.KeywordRepository.Add(entity);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
