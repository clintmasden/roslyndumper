using System;
using System.Linq;

namespace RoslynDumper.Extensions
{
    internal static class ObjectExtensions
    {
        internal static string GetFormattedVariableName(this object element)
        {
            if (element == null)
            {
                return "x";
            }

            var className = element.GetType().GetFormattedClassName();
            string variableName;

            var splitGenerics = className.Split('<');

            if (splitGenerics.Length > 2 || className.Contains(','))
            {
                variableName = splitGenerics[0];
            }
            else
            {
                variableName = className
                    .Replace("Nullable<", "OfNullable")
                    .Replace("<", "Of")
                    .Replace(">", "s")
                    .Replace(" ", "")
                    .Replace("[", "Array")
                    .Replace("]", "");
            }

            return variableName.ToLowerFirst();
        }

        internal static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || new[] {typeof(string), typeof(decimal), typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid)}
                .Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]);
        }
    }
}