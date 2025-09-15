using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetCityDropDown
{
    public class GetCityDropDownQuery : IRequest<List<GetCityDropDownDto>>
    {
        public GetCityDropDownQuery()
        {
        }
        public GetCityDropDownQuery(Guid? parentId)
        {
            ParentId = parentId;
        }
        public Guid? ParentId { get; private set; }
    }
}
