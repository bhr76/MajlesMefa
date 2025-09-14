using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.Common.Details
{
    public class SenatorDetailDto
    {
        public Guid UserId { get; set; }
        public Guid Id => UserId;

        public string Name { get; set; }

        public string Mobile { get; set; }
        public string Email { get; set; }

        public Guid ComissionMembership { get; set; }
        public string ComissionMembershipTitle { get; set; }

        public string SocailActivity { get; set; }

        public string SenaHistory { get; set; }

        public string JobHistory { get; set; }

        public string PoliticalTending { get; set; }

        public string PersonalFavorites { get; set; }

        public string FractionMembership { get; set; }

        public string SabegheHeyatReise { get; set; }

        public string SabegheEmzaEstizah { get; set; }

        public string Reshte { get; set; }

        public string ProfilePhoto { get; set; }

        public DateTime BirthDate { get; set; }

        public HozeEntekhabiEnum HozeEntekhabi { get; set; }
        public string HozeEntekhabiStr { get; set; }
        public int HozeEntekhabiInt { get { return (int)HozeEntekhabi; } set { HozeEntekhabi = (HozeEntekhabiEnum)value; } }

        public ShoghleGhalebEnum ShoghleGhaleb { get; set; }
        public int ShoghleGhalebInt { get { return (int)ShoghleGhaleb; } set { ShoghleGhaleb = (ShoghleGhalebEnum)value; } }


        public MadrakTahsiliEnum MadrakTahsili { get; set; }
        public int MadrakTahsiliInt { get { return (int)MadrakTahsili; } set { MadrakTahsili = (MadrakTahsiliEnum)value; } }


        public MahaleTahsilEnum MahaleTahsil { get; set; }
        public int MahaleTahsilInt { get { return (int)MahaleTahsil; } set { MahaleTahsil = (MahaleTahsilEnum)value; } }


        public GerayeshSiasiEnum GerayeshSiasi { get; set; }
        public int GerayeshSiasiInt { get { return (int)GerayeshSiasi; } set { GerayeshSiasi = (GerayeshSiasiEnum)value; } }


        public CityDto HozeCity { get; set; }
        public string HozeCityName { get; set; }

        public CityDto BirthCity { get; set; }

        public CityDto City { get; set; }
        public string CityName { get; set; }
    }
}
