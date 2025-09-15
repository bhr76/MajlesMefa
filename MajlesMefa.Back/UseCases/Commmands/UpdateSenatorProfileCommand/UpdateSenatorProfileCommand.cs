using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateSenatorProfileCommand
{
    public class UpdateSenatorProfileCommand: IRequest, IMapping
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string FractionMembership { get; set; }

        public string Mobile { get; set; }

        public Guid ComissionMembership { get; set; }

        public string SocailActivity { get; set; }

        public string SenaHistory { get; set; }

        public string JobHistory { get; set; }

        public string PoliticalTending { get; set; }

        public string PersonalFavorites { get; set; }
        
        public string SabegheEmzaEstizah { get; set; }

        public string SabegheHeyatReise { get; set; }

        public string Reshte { get; set; }

        public IFormFile? ProfilePhotoFile { get; set; }

        public string ProfilePhoto => this.ProfilePhotoFile?.FileName;
        
        public Guid HozeCityId { get; set; }

        public Guid BirthCityId { get; set; }

        public Guid CityId { get; set; }


        public HozeEntekhabiEnum HozeEntekhabi { get; set; }

        public ShoghleGhalebEnum ShoghleGhaleb { get; set; }

        public MadrakTahsiliEnum MadrakTahsili { get; set; }

        public MahaleTahsilEnum MahaleTahsil { get; set; }

        public GerayeshSiasiEnum GerayeshSiasi { get; set; }

        public DateTime BirthDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateSenatorProfileCommand, SenatorProfileEntity>();
        }
    }
}
