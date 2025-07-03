using System.Text.RegularExpressions;

namespace Masaafa.WebApi.Routing;

public class RouteTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is string val && !string.IsNullOrWhiteSpace(val))
            return Regex.Replace(val, "([a-z])([A-Z])", "$1-$2").ToLower();

        return null;
    }
}