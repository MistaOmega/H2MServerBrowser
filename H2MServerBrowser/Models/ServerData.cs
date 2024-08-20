using System;

namespace H2MServerBrowser.Models
{
    public class ServerData
    {
        public string? Hostname { get; set; }
        public string? Ip { get; set; }
        public int? Port { get; set; }
        public string? Gametype { get; set; }
        public string? Map { get; set; }
        public int? CurrentClientCount { get; set; }
        public int? MaxClientCount { get; set; }
        public long PingResult { get; set; }

        public string PingDisplay => PingResult == Int64.MaxValue ? "Pinging" : PingResult.ToString();

        public override string ToString()
        {
            return $"connect {Ip}:{Port}";
        }
    }
}