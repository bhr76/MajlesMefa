using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Specifications
{
    /// <summary>
    /// give last actionRefrence
    /// </summary>
    public class EditableDataEntrySpecification : ISpecification<ActionReferenceEntity>
    {
        public bool IsSatisfied(ActionReferenceEntity obj)
        {
            return
                obj.ActRefType == Enums.ActRefTypeEnum.Create
                || obj.ActRefType == Enums.ActRefTypeEnum.Edit;
        }
    }
}
