using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common.Details;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorProfileQuery
{
    public class GetSenatorProfileQueryHandler: IRequestHandler<GetSenatorProfileQuery, TableModel<SenatorDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSenatorProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<SenatorDetailDto>> Handle(GetSenatorProfileQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.SenatorRepository
                .NoTracking;
            if (request.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == request.UserId);
            }
            if (request.CommissionId != Guid.Empty && request.CommissionId != null)
            {
                query = query.Where(x => x.ComissionMembership == request.CommissionId);
            }
            var rslt = await query
                .Select(s => new SenatorDetailDto()
                {
                    ComissionMembership = s.ComissionMembership,
                    UserId = s.UserId,
                    Name = s.Name,
                    JobHistory = s.JobHistory,
                    Mobile = s.Mobile,
                    //PersonalFavorites = s.PersonalFavorites,
                    PoliticalTending = s.PoliticalTending,
                    //SocailActivity = s.SocailActivity,
                    SenaHistory = s.SenaHistory,
                    BirthDate = s.BirthDate,
                    //FractionMembership = s.FractionMembership,
                    GerayeshSiasi = s.GerayeshSiasi,
                    HozeEntekhabiStr=s.HozeEntekhabi.GetPersianName(),
                    HozeEntekhabi = s.HozeEntekhabi,
                    MadrakTahsili = s.MadrakTahsili,
                    //MahaleTahsil = s.MahaleTahsil,
                    Reshte = s.Reshte,
                    SabegheEmzaEstizah = s.SabegheEmzaEstizah,
                    SabegheHeyatReise = s.SabegheHeyatReise,
                    //ShoghleGhaleb = s.ShoghleGhaleb,
                    ProfilePhoto= s.ProfilePhoto,
                    CityName = s.City.Name,
                    HozeCityName= s.HozeCity.Name,
                    BirthCity = s.BirthCity == null ? null : new CityDto()
                    {
                        Id = s.BirthCityId,
                        Name = s.BirthCity.Name,
                        ParentId = s.BirthCity.ParentCityId,
                        ParentName = s.BirthCity.Parent.Name
                    },
                    City = new CityDto()
                    {
                        Id = s.CityId,
                        Name = s.City.Name,
                        ParentId = s.City.ParentCityId,
                        ParentName = s.City.Parent.Name

                    },
                    HozeCity = s.HozeCityId == null ? null :new CityDto()
                    {
                        Id = s.HozeCityId.Value,
                        Name = s.HozeCity.Name,
                        ParentId = s.HozeCity.ParentCityId,
                        ParentName = s.HozeCity.Parent.Name
                    }
                }).ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
