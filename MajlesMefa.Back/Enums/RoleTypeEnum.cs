using MajlesMefa.Back.Dtos.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums
{
    public enum RoleTypeEnum: byte
    {
        [Display(Name = "نماینده مجلس")]
        Senator =1,

        [Display(Name = "ادمین وزارت خانه")]
        MinistryAdmin = 2,

        [Display(Name = "کاربر وزارت خانه")]
        MinistryMember = 3,

        [Display(Name = "سازمان طرف مکاتبه")]
        Organization = 4,
        
        [Display(Name = "ادمین سامانه")]
        Admin = 5,

    }

    public static class RoleTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)RoleTypeEnum.Admin ,
                    RoleTypeEnum.Admin.GetPersianName()),
                IdNameDto.Create(
                    (int)RoleTypeEnum.MinistryAdmin ,
                    RoleTypeEnum.MinistryAdmin.GetPersianName()),
                 IdNameDto.Create(
                    (int)RoleTypeEnum.MinistryMember ,
                    RoleTypeEnum.MinistryMember.GetPersianName()),
                 IdNameDto.Create(
                    (int)RoleTypeEnum.Organization ,
                    RoleTypeEnum.Organization.GetPersianName()),
            };
        }
    }
    public static class AllRoleTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)RoleTypeEnum.MinistryAdmin ,
                    RoleTypeEnum.MinistryAdmin.GetPersianName()),
                 IdNameDto.Create(
                    (int)RoleTypeEnum.MinistryMember ,
                    RoleTypeEnum.MinistryMember.GetPersianName()),
                 IdNameDto.Create(
                    (int)RoleTypeEnum.Organization ,
                    RoleTypeEnum.Organization.GetPersianName()),
                 IdNameDto.Create(
                    (int)RoleTypeEnum.Admin ,
                    RoleTypeEnum.Admin.GetPersianName()),
                 IdNameDto.Create(
                    (int)RoleTypeEnum.Senator ,
                    RoleTypeEnum.Senator.GetPersianName()),
            };
        }
    }

    public static class RoleTypeEnumExtension
    {
        public static string GetPersianName(this RoleTypeEnum entityType)
        {
            switch (entityType)
            {
                case RoleTypeEnum.MinistryAdmin: return "ادمین وزارت خانه";
                case RoleTypeEnum.MinistryMember: return "کاربر وزارت خانه";
                case RoleTypeEnum.Organization: return "سازمان طرف مکاتبه";
                case RoleTypeEnum.Admin: return "ادمین";
                case RoleTypeEnum.Senator: return "نماینده";
                default: throw new Exception("تعریف نشده");
            }
        }
    }

}
