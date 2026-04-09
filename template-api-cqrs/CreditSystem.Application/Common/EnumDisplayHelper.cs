using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Template.Application.Common;

public static class EnumDisplayHelper
{
    /// <summary>Gets the [Display(Name)] for the enum value, or ToString() if not set.</summary>
    public static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null)
            return value.ToString();

        var display = field.GetCustomAttribute<DisplayAttribute>();
        return display?.GetName() ?? value.ToString();
    }
}
