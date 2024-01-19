using System.ComponentModel.DataAnnotations;

namespace Consultorio.Models
{
    public class Constants
    {
        public const string Admin = "ADMIN";
    }
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var displayAttribute = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                as DisplayAttribute[];

            return displayAttribute[0].Name;
        }
    }
}
