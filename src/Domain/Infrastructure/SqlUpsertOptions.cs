using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GasWeb.Domain.Infrastructure
{
    internal class SqlUpsertOptions<T>
    {
        public IReadOnlyCollection<string> Columns { get; private set; }
        public IReadOnlyCollection<string> OnConflictColumns { get; private set; }
        public Func<T, IEnumerable<object>> ValuesSelector { get; private set; }

        public SqlUpsertOptions<T> WithColumns(params Expression<Func<T, object>>[] columnSelectors)
        {
            Columns = columnSelectors.Select(GetColumnName).ToList();
            return this;
        }

        public SqlUpsertOptions<T> ConflictOn(params Expression<Func<T, object>>[] columnSelectors)
        {
            OnConflictColumns = columnSelectors.Select(GetColumnName).ToList();
            return this;
        }

        public SqlUpsertOptions<T> SelectValues(Func<T, IEnumerable<object>> valuesSelector)
        {
            ValuesSelector = valuesSelector;
            return this;
        }

        private static string GetColumnName(Expression<Func<T, object>> selector)
        {
            var propertyExpression = (selector.Body as UnaryExpression)?.Operand as MemberExpression;
            return propertyExpression?.Member.Name ?? throw new ArgumentException("Must be a member selector", nameof(selector));
        }
    }
}
