using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace H2MServerBrowser.Models;

public abstract class ApiResponse
{
    public class Instance
    {
        [JsonPropertyName("servers")] public List<Server> Servers { get; set; }
    }

    public class Server
    {
        [JsonPropertyName("ip")] public string? Ip { get; set; }

        [JsonPropertyName("clientnum")] public int ClientNum { get; set; }

        [JsonPropertyName("gametype")] public string? Gametype { get; set; }

        [JsonPropertyName("id")] public long Id { get; set; }

        [JsonPropertyName("maxclientnum")] public int MaxClientNum { get; set; }

        [JsonPropertyName("port")] public int Port { get; set; }

        [JsonPropertyName("map")] public string? Map { get; set; }

        [JsonPropertyName("version")] public string? Version { get; set; }

        [JsonPropertyName("game")] public string? Game { get; set; }

        [JsonPropertyName("hostname")] public string? Hostname { get; set; }
    }
}