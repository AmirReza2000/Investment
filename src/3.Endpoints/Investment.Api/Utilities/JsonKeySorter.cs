using System.Text.Json;

namespace Investment.Api.Utilities;

public static class JsonKeySorter
{
    public static string SortJsonKeys(string json, bool indented)
    {
        using var doc = JsonDocument.Parse(json);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = indented });

        WriteSorted(doc.RootElement, writer);

        writer.Flush();
        return System.Text.Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void WriteSorted(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();

                foreach (var prop in element.EnumerateObject().OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase))
                {
                    writer.WritePropertyName(prop.Name);
                    WriteSorted(prop.Value, writer);
                }

                writer.WriteEndObject();
                break;

            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                    WriteSorted(item, writer);
                writer.WriteEndArray();
                break;

            default:
                element.WriteTo(writer);
                break;
        }
    }

    public static bool TrySortJsonKeys(string jsonStrig, bool indented, out string jsonStrigSorted)
    {
        jsonStrigSorted = string.Empty;

        try
        {
            jsonStrigSorted = SortJsonKeys(jsonStrig, indented);

            return true;

        }
        catch
        {
            return false;
        }
    }
}
