using MediatR;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetOrganizationDropDown
{
    public class GetOrganizationDropDownQuery : IRequest<List<GetOrganizationDropDownDto>>
    {
        public GetOrganizationDropDownQuery()
        {
        }
        public GetOrganizationDropDownQuery(Guid? parentId)
        {
            ParentId = parentId;
        }
        public Guid? ParentId { get; private set; }
    }
}
