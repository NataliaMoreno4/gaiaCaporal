using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PlantillaBlazor.Domain.Enums
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())[0]
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName() ?? enumValue.ToString();
        }

        public static List<string> GetEnumDisplayNames<T>() where T : Enum
        {
            List<string> displayNames = new List<string>();
            foreach (var enumValue in Enum.GetValues(typeof(T)))
            {
                displayNames.Add(((Enum)enumValue).GetDisplayName());
            }
            return displayNames;
        }
    }
}
