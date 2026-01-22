using System.Text.Json;
using System.Text.Json.Nodes;
using AdService.Application.Abstractions.Helpers;

namespace AdService.Infrastructure.Core.Helpers;

public class MergePatchHelper : IMergePatchHelper
{
    public T ApplyMergePatch<T>(T dto, JsonObject patch, JsonSerializerOptions? options = null)
    {
        options ??= JsonSerializerOptions.Web;

        var target = JsonSerializer.SerializeToNode(dto, options) as JsonObject ?? new JsonObject();

        Merge(patch, target);

        var patchedDto = target.Deserialize<T>(options) ??
                         throw new InvalidOperationException("Failed to deserialize merged JSON to DTO");

        return patchedDto;
    }

    private void Merge(JsonObject patch, JsonObject target)
    {
        foreach (var item in patch)
        {
            var key = item.Key;
            var jsonNode = item.Value;


            if (jsonNode is null || (jsonNode is JsonValue jv && jv.GetValue<object?>() is null))
            {
                target.Remove(key);
                continue;
            }


            if (jsonNode is JsonObject patchChild && target[key] is JsonObject targetChild)
            {
                Merge(patchChild, targetChild);
            }
            else
            {
                target[key] = jsonNode.DeepClone();
            }
        }
    }
}