namespace csi5112group1project_service.Utils;
using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class ObjectIdConverter : JsonConverter<ObjectId>
{
  public override ObjectId Read(
      ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      => new(JsonSerializer.Deserialize<string>(ref reader, options));

  public override void Write(
      Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
      => writer.WriteStringValue(value.ToString());
}