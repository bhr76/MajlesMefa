using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand
{
    public class DeleteDataEntryCommandHandler : IRequestHandler<DeleteDataEntryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteDataEntryCommandHandler(IUnitOfWork unitOfWork, 
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteDataEntryCommand request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();
            //var entity = await _unitOfWork.DataEntryRepository
            //    .Tracking
            //    .Include(x => x.ActionReferences)
            //    .Include(x => x.Peygiries)
            //    .Where(x => x.CreatorId == cuser.BussinessUserId)
            //    //.Where(x => !x.ActionReferences.Any(x => x.FromUserId != cuser.BussinessUserId))
            //    .Where(x => x.Id == request.Id)
            //    .SingleOrDefaultAsync();
            var isExists = await _unitOfWork.DataEntryRepository
                .NoTracking
                .Include(x => x.ActionReferences)
                .Include(x => x.Peygiries)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

   

                if (isExists == null)
                {
                    throw new NullReferenceException("موجودیت یافت نشد");
                }
                else if(!cuser.Roles.Any(r => r.Equals(RoleTypeEnum.MinistryMember)) &&
                    !cuser.Roles.Any(r => r.Equals(RoleTypeEnum.MinistryAdmin)) &&
                    !cuser.Roles.Any(r => r.Equals(RoleTypeEnum.Admin))
                    )
                {
                    throw new AccessViolationException("تنها در صورت قادر به حذف خواهید بود که،" +
                                            " خودتان موجودیت مورد نظر را ایجاد کرده باشید" +
                                            " و به کسی ارجاع نشده باشد، در صورت ارجاع باید ارجاعات حذف شوند");
                }
                              
            
            if (cuser.Roles.Any(r => r.Equals(RoleTypeEnum.Senator)) || cuser.Roles.Any(r => r.Equals(RoleTypeEnum.Organization)))
            {
                if(isExists.ActionReferences.Any((x => x.FromUserId != cuser.BussinessUserId)))
                {
                    throw new AccessViolationException("شما قادر به حذف موجودیتی که توسط کاربران دیگر ویرایش یا ارجاع شده است، نمیباشید.");
                }
            }
           
            foreach(var item in isExists.Peygiries)
            {
                _unitOfWork.PeygiriRepository.Delete(item);
            }
            foreach(var item in isExists.ActionReferences)
            {
                _unitOfWork.ActionReferenceRepository.Delete(item);
            }
            

             _unitOfWork.DataEntryRepository.Delete(isExists);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
