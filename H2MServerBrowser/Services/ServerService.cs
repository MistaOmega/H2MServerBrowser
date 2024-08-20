using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using H2MServerBrowser.Models;
using HtmlAgilityPack;

namespace H2MServerBrowser.Services;

public class ServerService
{
    private readonly HtmlWeb _web = new();
    public async Task<List<ServerData>> FetchServerDataAsync(string url)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);
        var instances = JsonSerializer.Deserialize<List<ApiResponse.Instance>>(response);

        var list = new List<ServerData>();
        if(instances == null)
        {
            return list;
        }

        foreach (var server in from instance in instances from server in instance.Servers where server.Game == "H2M" select server)
        {
            if(server.Hostname == null || server.Ip == null || server.Port == 0 || server.Ip == "localhost")
            {
                continue;
            }

            var filteredHostname = Regex.Replace(server.Hostname, @"\^[a-zA-Z0-9]|:", string.Empty).Replace("^", "");
            list.Add(new ServerData
            {
                Hostname = filteredHostname,
                Ip = server.Ip,
                Port = server.Port,
                Gametype = server.Gametype,
                Map = server.Map,
                CurrentClientCount = server.ClientNum,
                MaxClientCount = server.MaxClientNum,
                PingResult = Int64.MaxValue // Pinging
            });
        }
        PingServers(list);
        return list;
    }

    private async void PingServers(List<ServerData> servers)
    {
        var tasks = new List<Task>();
        foreach (var server in servers)
        {
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    if(server.Ip == null)
                    {
                        return;
                    }
                    using var ping = new Ping();
                    var reply = await ping.SendPingAsync(server.Ip, 5000);
                    server.PingResult = reply.Status == IPStatus.Success ? reply.RoundtripTime : 999;
                }
                catch
                {
                    server.PingResult = 999;
                }
            }));
        }
        await Task.WhenAll(tasks);
    }
}