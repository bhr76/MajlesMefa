using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Specifications
{
    public interface ISpecification<TObject>
    {
        bool IsSatisfied(TObject obj);
    }
}
