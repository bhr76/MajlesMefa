using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.Pagination
{
    public class PagerDto
    {
        private int _page = 1;
        private int _pageSize = 20;
        private int _skip = 0;

        public int Skip
        {
            get
            {
                return (_page - 1) * _pageSize;
            }
            set
            {
                _skip = value;
                if (_pageSize > 0)
                {
                    _page = _skip / _pageSize;
                    if (_skip % _pageSize > 0)
                    {
                        _page++;
                    }
                }
            }
        }

        public int Take
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                _page = _skip / _pageSize;
                if (_skip % _pageSize > 0)
                {
                    _page++;
                }
            }
        }

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                _skip = (_page - 1) * _pageSize;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                _skip = (_page - 1) * _pageSize;
            }
        }
    }
}
