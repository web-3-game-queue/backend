﻿using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using GameQueue.Api.Contracts.Models;

namespace GameQueue.Api.Contracts.Responses;

public sealed record MapResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("maxPlayersCount")]
    public int MaxPlayersCount { get; set; }

    [JsonPropertyName("coverImageUrl")]
    public string? CoverImageUrl { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("mapStatus")]
    public MapStatusApi Status { get; set; } = MapStatusApi.Pending;
}
