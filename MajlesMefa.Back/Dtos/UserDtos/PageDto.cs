using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MajlesMefa.Back.Dtos.UserDtos
{
    public class PageDto
    {
        public Guid Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string IconUrl { get; set; }

    }
}
