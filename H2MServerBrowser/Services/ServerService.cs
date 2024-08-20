using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using H2MServerBrowser.Models;
using HtmlAgilityPack;

namespace H2MServerBrowser.Services;

public class ServerService
{
    private readonly HtmlWeb _web = new();

    public async Task<List<ServerData>> FetchServerDataAsync(string url)
    {
        var doc = await _web.LoadFromWebAsync(url);
        var h2MServers = doc.GetElementbyId("H2M_Servers");
        var serverNodes = h2MServers.Descendants("tr");

        var list = new List<ServerData>();
        foreach (var node in serverNodes)
        {
            var ip = node.GetAttributeValue("data-ip", string.Empty);
            var port = node.GetAttributeValue("data-port", string.Empty);

            if (!string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(port))
            {
                list.Add(new ServerData
                {
                    Hostname = node.SelectSingleNode(".//td[@data-hostname]").InnerText.Trim(),
                    Ip = ip,
                    Port = int.Parse(port),
                    Gametype = node.SelectSingleNode(".//td[@data-gametype]").InnerText.Trim(),
                    Map = node.SelectSingleNode(".//td[@data-map]").InnerText.Trim(),
                    CurrentClientCount = int.Parse(node.SelectSingleNode(".//td[@data-clientnum]").InnerText.Split('/')[0].Trim()),
                    MaxClientCount = int.Parse(node.SelectSingleNode(".//td[@data-clientnum]").InnerText.Split('/')[1].Trim()),
                    PingResult = Int64.MaxValue // Pinging
                });
            }
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