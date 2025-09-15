using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums
{
    public enum RefTypeEnum: short
    {
        [Display(Name = "جهت اطلاع")]
        JahateEtela = 1,

        [Display(Name = "جهت اقدام")]
        JahateEghdam = 2,

        [Display(Name = "جهت اقدام فوری")]
        JahateEghdameFori = 3,

        [Display(Name = "جهت پیگیری")]
        JahatePeygiri = 4,
        
        [Display(Name = "جهت تست")]
        JahateTest = 5,
        
        [Display(Name = "جهت اقدام مقتضی")]
        JahateEghdameMoghtazi = 6,

        [Display(Name = "جهت استحضار")]
        JahateEstehzar = 7,
    }
}
