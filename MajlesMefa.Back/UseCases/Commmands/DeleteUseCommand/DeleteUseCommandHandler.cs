using IdentityContext.Services.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteUseCommand
{
    public class DeleteUseCommandHandler : IRequestHandler<DeleteUseCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagerService _userManagerService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteUseCommandHandler(IUserManagerService userManagerService,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _userManagerService = userManagerService;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteUseCommand request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();
            var user = await _unitOfWork.UserRepository
                .FindAsync(request.UserId);

            if (user.CreatorUserId != cuser.UserId && user.Id != cuser.UserId)
            {
                if (cuser.Roles.Any(x => x != RoleTypeEnum.Admin))
                {
                    if (cuser.Roles.Any(x => x != RoleTypeEnum.MinistryAdmin))
                    {
                        throw new AccessViolationException("تنها اجازه ی ویرایش کاربری که تعریف نموده اید را دارید");
                    }
                    else
                    {
                        var roles = await _userManagerService.GetUserRolesAsync(user.UserId.ToString());
                        if (roles.Contains(RoleTypeEnum.Admin.ToString()))
                        {
                            throw new AccessViolationException("اجاره ی ویرایش کاربر سطح بالاتر را ندارید");
                        }
                    }
                }
            }

            var hasRelatedDatas = _unitOfWork.UserRepository
                .NoTracking
                .Where(x => x.UserId == request.UserId)
                .Any(x => x.CreatorDataEntries.Any() || x.FromActionReferences.Any() || x.ToActionReferences.Any() || x.SenatorsDataEntries.Any());
            if (hasRelatedDatas)
            {
                throw new AccessViolationException("ابتدا کلیه اقدامات و ارجاعات و اطلاعات وابسته را حذف کنید");
            }
            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _userManagerService.DeleteUserAsync(user.UserId);
        }
    }
}
