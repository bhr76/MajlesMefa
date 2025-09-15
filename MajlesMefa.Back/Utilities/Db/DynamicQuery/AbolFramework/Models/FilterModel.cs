using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models
{
    public class FilterModel
    {
        public static class Operators
        {
            public const string Equalss = "=";
            public const string NotEquals = "!=";
            public const string LittleThan = "<";
            public const string LittleEqualThan = "<=";
            public const string GreeterThan = ">";
            public const string GreeterEqualThan = ">=";
            public const string Startswith = "=StartsWith";
            public const string EndsWith = "EndsWith";
            public const string Contains = "Contains";
        }


        /// <summary>
        /// Gets or sets the name of the sorted field (property). Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the filtering operator. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the filtering value. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the filtering logic. Can be set to "or" or "and". Set to <c>null</c> unless <c>Filters</c> is set.
        /// </summary>
        public string Logic { get; set; } = "and";

        /// <summary>
        /// Gets or sets the child filter expressions. Set to <c>null</c> if there are no child expressions.
        /// </summary>
        public IEnumerable<FilterModel> Filters { get; set; }

        public int Tst { get; set; }


        /// <summary>
        /// Mapping of Kendo DataSource filtering operators to Dynamic Linq
        /// </summary>
        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"}
        };

        /// <summary>
        /// Get a flattened list of all child filter expressions.
        /// </summary>
        public IList<FilterModel> All()
        {
            var filters = new List<FilterModel>();

            Collect(filters);

            return filters;
        }

        private void Collect(ICollection<FilterModel> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (var filter in Filters)
                {
                    filters.Add(filter);

                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        /// <summary>
        /// Converts the filter expression to a predicate suitable for Dynamic Linq e.g. "Field1 = @1 and Field2.Contains(@2)"
        /// </summary>
        /// <param name="filters">A list of flattened filters.</param>
        public string ToExpression(IList<FilterModel> filters)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" + string.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            var index = filters.IndexOf(this);

            string comparison;
            if (!operators.Keys.Contains(Operator))
            {
                if (operators.Values.Any(v => v == Operator))
                    comparison = Operator;
                else
                    throw new Exception("invalid operator");
            }
            else
            {
                comparison = operators[Operator];
            }
            if (Operator == "doesnotcontain")
            {
                return string.Format("!{0}.{1}(@{2})", Field, comparison, index);
            }

            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return string.Format("{0}.{1}(@{2})", Field, comparison, index);
            }

            return string.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }
}
