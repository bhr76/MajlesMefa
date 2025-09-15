using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetUserMenuQuery
{
    public class GetUserMenuQueryHandler : IRequestHandler<GetUserMenuQuery, List<PageDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserMenuQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<List<PageDto>> Handle(GetUserMenuQuery request, CancellationToken cancellationToken)
        {
            var currentRoles = _currentUserService.GetCurrentUser().Roles;
            var rslt =  _unitOfWork.RoleRepository
                .NoTracking
                .Where(x => currentRoles.Contains(x.RoleType))
                .SelectMany(x => x.PageRoles)
                .Select(x => new PageDto()
                {

                    Action = x.Page.Action,
                    Controller = x.Page.Controller,
                    Url = x.Page.Url,
                    Title = x.Page.Title,
                    IconUrl = x.Page.IconUrl,
                }).Distinct().OrderBy(x => x.IconUrl);


            var homepage = rslt.FirstOrDefault(x => x.Url == "/Home");

            var rsltList = await rslt.ToListAsync(cancellationToken);
            var homeIndex = rsltList.FindIndex(x => x.Url == "/Home");

            rsltList.RemoveAt(homeIndex);
            rsltList.Insert(0, homepage);

            return rsltList;


            //        Action = x.Page.Action,
            //        Controller = x.Page.Controller,
            //        Url = x.Page.Url,
            //        Title = x.Page.Title,
            //        IconUrl = x.Page.IconUrl,
            //    }).Distinct().ToListAsync(cancellationToken);



            //return rslt;
        }
    }
}
