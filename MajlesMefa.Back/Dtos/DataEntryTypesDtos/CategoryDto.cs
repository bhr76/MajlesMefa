using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos
{
    public class CategoryDto
    {
        public string Name { get; set; }

        public int SubCategoriesCount { get; set; }

        public Guid Id { get; set; }
        
        public Guid ParentId { get; set; }

        public bool IsCentralOffice { get; set; }

        [Display(Name = "نیاز به فیلد مبلغ دارد؟")]
        public string IsCentralOfficeStr => IsCentralOffice ? "بله" : "خیر";

    }
}
