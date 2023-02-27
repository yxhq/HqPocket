using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HqPocket.Helpers;

public static class JsonHelper
{
    /// <summary>
    /// 将属性和属性值转换为Json字符串格式
    /// </summary>
    /// <param name="propertyInfo">属性</param>
    /// <param name="value">属性值</param>
    /// <returns>"属性名称:属性值"</returns>
    public static string GetJsonString(PropertyInfo propertyInfo, string value)
    {
        StringBuilder stringBuilder = new($"\"{propertyInfo.Name}\":");
        return Type.GetTypeCode(propertyInfo.PropertyType) switch
        {
            TypeCode.DateTime or TypeCode.String or TypeCode.Object => stringBuilder.Append(value.StartsWith("\"") ? value : $"\"{value}\"").ToString(),
            _ => stringBuilder.Append(value).ToString()
        };
    }

    public static void Serialize<T>(string jsonFile, T obj)
    {
        using StreamWriter streamWriter = new(jsonFile);
        string json = JsonSerializer.Serialize(obj);
        streamWriter.Write(json);
    }

    public static T? Deserialize<T>(string jsonFile)
    {
        using StreamReader streamReader = new(jsonFile);
        string json = streamReader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
    }

    public static async Task SerializeAsync<T>(string jsonFile, T obj)
    {
        await using StreamWriter streamWriter = new(jsonFile);
        string json = JsonSerializer.Serialize(obj);
        await streamWriter.WriteAsync(json);
    }

    public static async Task<T?> DeserializeAsync<T>(string jsonFile)
    {
        using StreamReader streamReader = new(jsonFile);
        string json = await streamReader.ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(json);
    }
}
