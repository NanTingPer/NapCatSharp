using System.Runtime.CompilerServices;
using System.Text.Json;

namespace NapCatSharp.RequestModels;

/// <summary>
/// 像NapCat Http Server发送请求的数据模型基类
/// </summary>
public abstract class RequestModelBase
{
    /// <summary>
    /// 初始化终结点映射字典
    /// </summary>
#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    [ModuleInitializer]
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    internal static void InitInterfaceMap()
    {
        Type thisType = typeof(RequestModelBase);
        var endpointMapArray = thisType.Assembly.GetTypes()
            .Where(type => type.IsAssignableTo(thisType) && type.IsAbstract==false && type.IsInterface == false)
            .Select(type => (Type: type, Endpoint: StandardizedEndpoint(((RequestModelBase)Activator.CreateInstance(type)!).GetEndpoint())))
            .ToArray()
            ;

        for (int i = 0; i < endpointMapArray.Length; i++) {
            var endpointMap = endpointMapArray[i];
            EndpointMap[endpointMap.Type] = endpointMap.Endpoint;
        }
    }

    /// <summary>
    /// 标准化终结点，如果其有 '/' 开头则原样返回，否则添加。
    /// </summary>
    private static string StandardizedEndpoint(string endpoint)
    {
        if(endpoint.StartsWith('/'))
            return endpoint;
        return '/' + endpoint;
    }

    public static readonly Dictionary<Type, string> EndpointMap = [];

    public virtual string ToJson(JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(this, GetType(), options);
    }

    /// <summary>
    /// <code>
    /// new StringContent(<see cref="ToJson"/>, System.Text.Encoding.UTF8, "application/json")
    /// </code>
    /// </summary>
    /// <returns></returns>
    public virtual StringContent ToJsonStringContent(JsonSerializerOptions? options = null)
    {
        return new StringContent(ToJson(options), System.Text.Encoding.UTF8, "application/json");
    }

    public override string ToString()
    {
        return ToJson();
    }

    public abstract string GetEndpoint();
}
