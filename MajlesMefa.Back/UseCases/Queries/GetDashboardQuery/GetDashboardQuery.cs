using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetDashboardQuery
{
    public class GetDashboardQuery: IRequest<List<DashboardItemDto>>
    {
    }

    public class GetDashboardMokatebeByStatusQuery : IRequest<List<DashboardMokatebeItemDto>>
    {
        public MokatebeTypeEnum? MokatebeType { get; set; }
        public GetDashboardMokatebeByStatusQuery(MokatebeTypeEnum? mokatebeType)
        {
            MokatebeType = mokatebeType;
        }
        public GetDashboardMokatebeByStatusQuery()
        {
        }
    }

    public class GetDashboardMolaghatByCityQuery : IRequest<List<DashboardItemDto>>
    {
    }

    public class GetDashboardMolaghatByMonthQuery : IRequest<List<DashboardItemDto>>
    {
    }

    public class GetDashboardSoalatByCityQuery : IRequest<List<DashboardItemDto>>
    {
    }
    public class GetDashboardSoalatByOrganizationQuery : IRequest<List<DashboardItemDto>>
    {
    }

    public class GetDashboardMokatebeByResponseStatusQuery : IRequest<List<DashboardMokatebeItemDto>>
    {
        public MokatebeTypeEnum? MokatebeType { get; set; }
        public GetDashboardMokatebeByResponseStatusQuery(MokatebeTypeEnum? mokatebeType)
        {
            MokatebeType = mokatebeType;
        }
        public GetDashboardMokatebeByResponseStatusQuery()
        {
        }
    }

    public class GetDashboardSoalatByStatusQuery : IRequest<List<DashboardItemDto>>
    {
    }

    public class GetDashboardMokatebeByOrganizationQuery : IRequest<List<MokatebeDashboardItemDto>>
    {
    }
}
