using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace EventSourcing.Application.Common.Resolvers;

/// <summary>
/// Custom Contract Resolver to Set Private property when Rehydrate Events
/// </summary>
public class PrivateSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var jsonProperty = base.CreateProperty(member, memberSerialization);

        if (jsonProperty.Writable)
            return jsonProperty;

        if (member is not PropertyInfo propertyInfo) return jsonProperty;

        var setter = propertyInfo.GetSetMethod(true);
        jsonProperty.Writable = setter is not null;

        return jsonProperty;
    }
}