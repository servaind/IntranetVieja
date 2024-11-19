using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for EnumExtensions
/// </summary>
public static class EnumExtensions
{
    public static string GetDescription<T>(this T value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        DescriptionAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    as DescriptionAttribute;

        return attribute == null ? value.ToString() : attribute.Description;
    }

    /// <summary>
    /// Only with enums.
    /// </summary>
    public static List<DataSourceItem> EnumToList<T>()
    {
        if (!typeof(T).IsEnum) throw new Exception("The method only accepts Enums.");

        return (from T t in Enum.GetValues(typeof(T))
                select new DataSourceItem(t.GetDescription(), Convert.ToInt32(t).ToString())).ToList();
    }
}