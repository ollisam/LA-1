using System.Dynamic;

namespace AudioPool.Models;

public static class HyperMediaExtensions
{
    public static void AddListReference(this ExpandoObject item, string key, IEnumerable<string> list) =>
        (item as IDictionary<string, object>)
        .Add(key, list.Select(l => new LinkRepresentation { Href = l }));

    public static void AddReference(this ExpandoObject item, string key, string value) =>
        (item as IDictionary<string, object>)
        .Add(key, new LinkRepresentation { Href = value! });
}