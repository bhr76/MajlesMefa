using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateMenuCommand
{
    public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMenuCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            var page = await _unitOfWork.PageRepository
                .Tracking
                .Include(x => x.Roles)
                .Where(x => x.Id == request.PageId)
                .SingleOrDefaultAsync(cancellationToken);
            
            if(request.Roles == null)
            {
                foreach (var item in page.Roles)
                {
                    await _unitOfWork.PageRepository.RemoveRoleAsync(page.Id, item.RoleId);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return;
            }
            var roles = await _unitOfWork.RoleRepository
                .NoTracking
                .Where(r => request.Roles.Contains(r.RoleType))
                .Select(x => new
                {
                    x.Id,
                    x.RoleType
                }).ToListAsync(cancellationToken);
            var mustRemove = page.Roles.Where(x => !roles.Any(xx => xx.Id == x.RoleId));
            var mustAdd = roles.Where(x => !page.Roles.Any(xx => xx.RoleId == x.Id))
                .Select(x => new PageRoleEntity()
                {
                    PageId = page.Id,
                    RoleId = x.Id
                });
            foreach(var item in mustRemove)
            {
                await _unitOfWork.PageRepository.RemoveRoleAsync(page.Id, item.RoleId);
            }
            foreach(var item in mustAdd)
            {
                _unitOfWork.PageRepository.AddRole(page.Id, item.RoleId);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
