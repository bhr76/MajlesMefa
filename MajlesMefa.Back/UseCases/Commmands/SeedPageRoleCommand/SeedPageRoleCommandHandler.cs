using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.SeedPageRoleCommand
{
    public class SeedPageRoleCommandHandler : IRequestHandler<SeedPageRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeedPageRoleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SeedPageRoleCommand request, CancellationToken cancellationToken)
        {
            var id = await _unitOfWork.UserRepository
                .NoTracking
                .Where(x => x.UserRoles.Any(r => r.Role.RoleType == RoleTypeEnum.Admin))
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();
            Assembly asm = request.Assembly;

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(x => x.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType == typeof(AuthAttribute))
                    || x.CustomAttributes.Any(attr => attr.AttributeType == typeof(AuthAttribute)))
                    .Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(DisplayAttribute)))
                    .Where(x =>
                        x.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpGetAttribute)) ||
                        (
                        !x.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpPostAttribute)) &&
                        !x.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpPutAttribute)) &&
                        !x.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpDeleteAttribute))
                     ))
                    .Where(x => !x.GetParameters().Any() || x.GetParameters().Any(xx =>
                        xx.ParameterType == typeof(CancellationToken)
                        || Nullable.GetUnderlyingType(xx.ParameterType) != null))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        AuthAttr = x.CustomAttributes.Any(attr => attr.AttributeType == typeof(AuthAttribute)) ?
                            x.GetCustomAttribute<AuthAttribute>()
                            : x.DeclaringType.GetCustomAttribute<AuthAttribute>(),
                        Display = x.GetCustomAttribute<DisplayAttribute>().Name,
                    })
                    .Where(x => x.Action == "Index")
                    .ToList();
            var dbRoles = await _unitOfWork.RoleRepository
                .NoTracking
                .ToListAsync();
            var pages = controlleractionlist.Select(x => new
            {
                Page = new PageEntity()
                {
                    Controller = "/" + x.Controller.Replace("Controller", ""),
                    Action = x.Action,
                    CreatorUserId = id,
                    Url = request.BaseUrl + "/" + x.Controller.Replace("Controller", ""),
                    Title = x.Display,
                },
                Roles = x.AuthAttr.Arguments.ToList()
            }).ToList();
            foreach (var p in pages)
            {
                _unitOfWork.PageRepository.Add(p.Page);
                var roles = p.Roles.Cast<RoleTypeEnum[]>().SelectMany(x => x).ToList();
                if (roles?.Count == 0)
                {
                    roles = Enum.GetValues<RoleTypeEnum>().ToList();
                }
                else roles ??= new List<RoleTypeEnum>();

                foreach (var r in roles)
                {
                    _unitOfWork.PageRepository.AddRole(p.Page, dbRoles.SingleOrDefault(rr => rr.RoleType == r).Id);
                }
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
