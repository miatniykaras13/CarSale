using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Application.Abstractions.Helpers;

public interface IMergePatchHelper
{
    T ApplyMergePatch<T>(T currentDto, JsonObject patch, JsonSerializerOptions? options = null);
}