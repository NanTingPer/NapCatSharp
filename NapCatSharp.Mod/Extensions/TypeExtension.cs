using NapCatSharp.Mod.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Extensions;

public static class TypeExtension
{
    /// <summary>
    /// 返回此Type公开属性的SetValue
    /// </summary>
    public static PropertySets GetPropertySets(this Type type)
    {
        var propSets = new PropertySets();
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        // (object e_object, object value) => e_object.prop = value
        // e_object => Box(type)
        // value => Box(type)
        foreach (var propertyInfo in properties) {
            if(propertyInfo.GetSetMethod() == null) continue;
            if(propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() != null) continue; // 排除
            var e_object = Expression.Parameter(typeof(object)); // object e_object
            var e_type = Expression.Convert(e_object, type);  // (Type)e_object
            var e_expression = Expression.Property(e_type, propertyInfo); // ((Type)e_object).Prop
            
            var value_object = Expression.Parameter(typeof(object)); // object value
            var value_type = Expression.Convert(value_object, propertyInfo.PropertyType); // (Type)value 
            var assign_expression = Expression.Assign(e_expression, value_type); // ((Type)e_object).Prop = (Type)value
            var lambda = Expression.Lambda<Action<object, object>>(assign_expression, e_object, value_object).Compile();
            propSets.Add(propertyInfo.Name, lambda/*, propertyInfo.PropertyType*/);
        }
        return propSets;
    }
}