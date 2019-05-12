using System;
using System.Linq.Expressions;

namespace Granite
{
    public static class StringExtensions
    {
        public static string GetPropertyName<T, TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
    }
}
