using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SpearPoint.Infrastructure.Persistence.Converters;
public class StringListToJsonConverter : ValueConverter<List<string>, string>
{
    private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions();

    public StringListToJsonConverter() : base(
        v => JsonSerializer.Serialize(v ?? new List<string>(), DefaultOptions),
        v => string.IsNullOrWhiteSpace(v)
            ? new List<string>()
            : (JsonSerializer.Deserialize<List<string>>(v, DefaultOptions) ?? new List<string>()))
    { }
}
