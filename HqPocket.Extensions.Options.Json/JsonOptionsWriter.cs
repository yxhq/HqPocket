using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HqPocket.Extensions.Options.Json;

public class JsonOptionsWriter : OptionsWriter
{
    private readonly JsonSerializerOptions _options;
    public JsonOptionsWriter(IConfiguration configuration, IOptions<JsonSerializerOptions> options) : base(configuration)
    {
        _options = options.Value;
    }

    protected override void Write(string path, IDictionary<string, object?> optionsFactory)
    {
        using StreamWriter streamWriter = new(path);
        string json = JsonSerializer.Serialize(optionsFactory, _options);
        streamWriter.Write(json);
    }

    protected override IDictionary<string, object?>? Load(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(path))
        {
            return new Dictionary<string, object?>();
        }
        using StreamReader streamReader = new(path);
        string json = streamReader.ReadToEnd();
        return JsonSerializer.Deserialize<IDictionary<string, object?>>(json);
    }
}
